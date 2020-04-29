using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;
using TSWMod.TSW.Caltrains;
using TSWMod.TSW.CSX;
using TSWMod.TSW.DB;
using TSWMod.TSW.LIRR;
using TSWMod.TSW.NEC;
using Timer = System.Timers.Timer;

namespace TSWMod.TSW
{
    class TSWConnector
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public const string ProcessName = "TS2Prototype-Win64-Shipping";
        public static readonly string ProcessBase = ProcessName + ".exe+";
        
        private const ulong WorldOffset = 0x3CD4358; // Static pointer to world offset from process
        private const ulong WorldInfoOffset = 0x06f8; // name of the world from world;
        private const ulong Level = 0x0030; // Level offset from world
        private const ulong LevelObjectList = 0x00A0; // Pointer to object list from level offset
        private const ulong LevelObjectList2 = 0x00B0; // Pointer to another object list from level offset
        private const ulong LevelObjectListLength = 0x00A8; // Length of object list to check
        private const ulong LevelObjectListLength2 = 0x00B8; // Length of another object list to check
        private const ulong LocomotiveNameOffset = 0x0420;  // Pointer to locomotive name information

        private const string GenericMapNamePrefix = "GenericDiorama"; // Main menu world name

        private static readonly IDictionary<int, InputHelpers.VKCodes[]> GameControlKeys =  new Dictionary<int, InputHelpers.VKCodes[]>
        {
            {0, new []{InputHelpers.VKCodes.VK_E }},
            {1, new []{InputHelpers.VKCodes.VK_ESCAPE }},
            {14,new []{ InputHelpers.VKCodes.VK_1 }},
            {15,new []{ InputHelpers.VKCodes.VK_2 }},
            {16,new []{ InputHelpers.VKCodes.VK_3 }},
            {17,new []{ InputHelpers.VKCodes.VK_8 }},
            {18,new []{ InputHelpers.VKCodes.VK_F1 }},
            {19,new []{ InputHelpers.VKCodes.VK_9 }},

        };

        public TSWConnector(RailDriverConnector rd)
        {
            _rd = rd;
            _m = new Mem();
            _enumeratingTimer = new Timer(500) { AutoReset = true };
            _enumeratingTimer.Elapsed += EnumeratingTimerOnElapsed;
            _gameSessionChecker = new Timer(2000) { AutoReset = true };
            _gameSessionChecker.Elapsed += GameSessionCheckerOnElapsed;
            _trainEnumerator = new Timer(5000) { AutoReset = true };
            _trainEnumerator.Elapsed += TrainEnumeratorOnElapsed;
            _calibrationChecker = new Timer(500) {AutoReset = true};
            _calibrationChecker.Elapsed += CalibrationCheckerOnElapsed;
            _tswControlLoop = new Timer(20) {AutoReset = true};
            _tswControlLoop.Elapsed += TswControlLoopOnElapsed;
            _currentMap = "";
            _currentLocomotiveLock = new object();
            _locomotivesLock = new object();
            _foundLocomotives = new Dictionary<UIntPtr, ILocomotive>();
            rd.ButtonPressed += RdOnButtonPressed;
            rd.ButtonReleased += RdOnButtonReleased;
            KeyboardLayoutManager.Current = new USDefaultKeyboardLayout();

            var keyboard = System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.Name;
            if (keyboard == "fr-FR")
            {
                KeyboardLayoutManager.Current = new FrenchDefaultKeyboardLayout();
            }
        }

        public JObject GetCurrentLocomotiveConfig()
        {
            lock (_currentLocomotiveLock)
            {
                return _currentLocomotive?.GetConfiguration();
            }
        }

        public void SetCurrentLocomotiveConfig(JObject newConfig)
        {
            lock (_currentLocomotiveLock)
            {
                _currentLocomotive?.SetConfiguration(newConfig);
            }
        }

        private void RdOnButtonReleased(object sender, RailDriverButtonEvent e)
        {
            var mapping = _currentLocomotive?.GetButtonMappings();
            if (mapping != null && mapping.ContainsKey(e.KeyCode))
            {
                InputHelpers.KeyComboUp(_currentProcess.MainWindowHandle, mapping[e.KeyCode]);
            }
            else if (GameControlKeys.ContainsKey(e.KeyCode))
            {
                InputHelpers.KeyComboUp(_currentProcess.MainWindowHandle, GameControlKeys[e.KeyCode]);
            }
        }

        private void RdOnButtonPressed(object sender, RailDriverButtonEvent e)
        {
            if (e.KeyCode == 13 && _inSession)  // special key to change train
            {
                LockClearSetCurrentLocomotive();
                _foundLocomotives.Clear();
                _calibrationChecker.Start();
                _trainEnumerator.Start();
                StatusChanged?.Invoke(this, new TSWConnectionChangedEvent(TSWConnectorStatus.Calibrating));
            }
            else
            {
                var mapping = _currentLocomotive?.GetButtonMappings();
                if (mapping != null && mapping.ContainsKey(e.KeyCode))
                {
                    InputHelpers.KeyComboDown(_currentProcess.MainWindowHandle, mapping[e.KeyCode]);
                }
                else if (GameControlKeys.ContainsKey(e.KeyCode))
                {
                    InputHelpers.KeyComboDown(_currentProcess.MainWindowHandle, GameControlKeys[e.KeyCode]);
                }
            }
        }


        private void TswControlLoopOnElapsed(object sender, ElapsedEventArgs e)
        {

            try
            {
                lock (_currentLocomotiveLock)
                {
                    if (_currentLocomotive == null) // no current locomotive to control
                        return;
                    _currentLocomotive.OnControlLoop(_rd.CurrentLeverState, new int[] { });
                }

            }
            catch (Exception exception)
            {
                OnException(exception);
                Logger.Error("Error occured during control loop. Disconnecting from TSW. Restarting...");
                OpenConnection();
            }
        }

        private void CalibrationCheckerOnElapsed(object sender, ElapsedEventArgs e)
        {
            KeyValuePair<UIntPtr, ILocomotive>[] trainArray = new KeyValuePair<UIntPtr, ILocomotive>[] { };
            lock (_locomotivesLock)
            {
                trainArray = _foundLocomotives.ToArray();
            }

            foreach (var train in trainArray)
            {
                if (train.Value.CheckPlayerCalibration())
                {
                    _trainEnumerator.Stop();
                    _calibrationChecker.Stop();
                    Logger.Info($"Train connected at {train.Key.ToUInt64():x16}");
                    Thread.Sleep(1000);
                    LockClearSetCurrentLocomotive(train.Value);
                    StatusChanged?.Invoke(this, new TSWConnectionChangedEvent(TSWConnectorStatus.Ready));
                }
            }
        }

        private void OnException(Exception e)
        {
            Logger.Fatal(e);
            CloseConnection();
            StatusChanged?.Invoke(this, new TSWConnectionChangedEvent(TSWConnectorStatus.Error));
        }

        private void TrainEnumeratorOnElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var objectsInLevel = GetObjectList();

                foreach (var obj in objectsInLevel)
                {
                    AddIfPtrIsKnownLocomotive(obj);
                }

                StatusChanged?.Invoke(this, new TSWConnectionChangedEvent(TSWConnectorStatus.Calibrating));
            }
            catch (Exception exception)
            {
                OnException(exception);
            }
        }

        private void AddIfPtrIsKnownLocomotive(UIntPtr ptr)
        {
            if (_foundLocomotives.ContainsKey(ptr))
            {
                return;
            }

            var possibleName = _m.ReadUTF16(
                _m.GetCodeRepresentation(
                    _m.GetPtr(
                        _m.GetCodeRepresentation((UIntPtr)((ulong)ptr + LocomotiveNameOffset)))));


            // Check each known locomotive and initialize if not exist
            // Here are all the supported trains
            lock (_locomotivesLock)
            {
                if (possibleName.Contains(DB146_2.NamePartial))
                {
                    _foundLocomotives.Add(ptr, new DB146_2(_m, ptr, _currentProcess.MainWindowHandle));
                }
                else if (possibleName.Contains(DB185_2.NamePartial))
                {
                    _foundLocomotives.Add(ptr, new DB185_2(_m, ptr, _currentProcess.MainWindowHandle));
                }
                else if (possibleName.Contains(DB_766pbzfa.NamePartial))
                {
                    _foundLocomotives.Add(ptr, new DB_766pbzfa(_m, ptr, _currentProcess.MainWindowHandle));
                }
                else if (possibleName.Contains(AC4400CW.NamePartial))
                {
                    _foundLocomotives.Add(ptr, new AC4400CW(_m, ptr, _currentProcess.MainWindowHandle));
                }
                else if (possibleName.Contains(SD40_2.NamePartial))
                {
                    _foundLocomotives.Add(ptr, new SD40_2(_m, ptr, _currentProcess.MainWindowHandle));
                }
                else if (possibleName.Contains(GP38_2.NamePartial))
                {
                    if (possibleName.Contains(GP38_2.YN3NamePartial))
                    {
                        _foundLocomotives.Add(ptr, GP38_2.CreateNECVersion(_m, ptr, _currentProcess.MainWindowHandle));
                    }
                    else if (possibleName.Contains(GP38_2.SFJPartial))
                    {
                        _foundLocomotives.Add(ptr, GP38_2.CreateSFJVersion(_m, ptr, _currentProcess.MainWindowHandle));
                    }
                    else
                    {
                        _foundLocomotives.Add(ptr, GP38_2.CreateCSXHeavyHaulVersion(_m, ptr, _currentProcess.MainWindowHandle));
                    }
                }
                else if (possibleName.Contains(ACS_64.NamePartial))
                {
                    _foundLocomotives.Add(ptr, new ACS_64(_m, ptr, _currentProcess.MainWindowHandle));
                }
                else if (possibleName.Contains(M7.NamePartial))
                {
                    _foundLocomotives.Add(ptr, new M7(_m, ptr, _currentProcess.MainWindowHandle));
                }
                else if (possibleName.Contains(F40PH.NamePartial))
                {
                    _foundLocomotives.Add(ptr, new F40PH(_m, ptr, _currentProcess.MainWindowHandle));
                }
                else if (possibleName.Contains(NipponCab.NamePartial))
                {
                    _foundLocomotives.Add(ptr, new NipponCab(_m, ptr, _currentProcess.MainWindowHandle));
                }
            }
        }

        private void GameSessionCheckerOnElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (_mOpened)
                {
                    var mapName = GetMapName();
                    if (_currentMap != mapName)
                    {
                        Logger.Info($"Map changed, new map {mapName}");
                        _currentMap = mapName;
                        OnMapChanged();
                    }

                    if (_currentProcess.HasExited)
                    {
                        CloseConnection();
                        OpenConnection();
                    }
                }
            }
            catch (Exception exception)
            {
                OnException(exception);
            }
        }

        private void OnMapChanged()
        {
            if (_currentMap.Contains(GenericMapNamePrefix) || string.IsNullOrEmpty(_currentMap) || !_currentMap.StartsWith("/"))
            {
                _inSession = false;
                _calibrationChecker.Stop();
                _tswControlLoop.Stop();
                _trainEnumerator.Stop();
                LockClearSetCurrentLocomotive();
                _foundLocomotives.Clear();
                StatusChanged?.Invoke(this, new TSWConnectionChangedEvent(TSWConnectorStatus.WaitingForGame));
                MapChanged?.Invoke(this, new TSWMapChangedEvent("No Active Map"));
            }
            else
            {
                _inSession = true;
                MapChanged?.Invoke(this, new TSWMapChangedEvent(_currentMap.Split('/').Last()));
                _trainEnumerator.Start();
                _calibrationChecker.Start();
                _tswControlLoop.Start();
            }
        }

        private void EnumeratingTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            var p = FindTSWProcess();
            if (p != null)
            {
                _mOpened = _m.OpenProcess(p.Id);
                if (_mOpened)
                {
                    _enumeratingTimer.Stop();
                    _currentProcess = p;
                    StatusChanged?.Invoke(this, new TSWConnectionChangedEvent(TSWConnectorStatus.WaitingForGame));
                    _gameSessionChecker.Start();
                }
                else
                {
                    Logger.Warn("Error opening process for Memory, trying again");
                }
            }
        }

        private List<UIntPtr> GetObjectList()
        {
            var objectList = new List<UIntPtr>();
            try
            {
                var world = _m.GetPtr(ProcessBase + WorldOffset.ToString("x16"));
                var level = _m.GetPtr(((ulong)world + Level).ToString("x16"));
                var levelObjectList = _m.GetPtr(((ulong)level + LevelObjectList).ToString("x16"));
                var levelObjectListLength = _m.ReadInt($"0x{((ulong)level + LevelObjectListLength):x16}");
                for (ulong i = 0; i < (ulong)levelObjectListLength; i++)
                {
                    var code = _m.GetCodeRepresentation((UIntPtr)((ulong)levelObjectList + i * 8));
                    var ptr = (UIntPtr)_m.ReadLong(code);
                    objectList.Add(ptr);
                }

                var levelObjectList2 = _m.GetPtr(((ulong)level + LevelObjectList2).ToString("x16"));
                var levelObjectListLength2 = _m.ReadInt($"0x{((ulong)level + LevelObjectListLength2):x16}");
                for (ulong i = 0; i < (ulong)levelObjectListLength2; i++)
                {
                    var code = _m.GetCodeRepresentation((UIntPtr)((ulong)levelObjectList2 + i * 8));
                    var ptr = (UIntPtr)_m.ReadLong(code);
                    objectList.Add(ptr);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error during get object list. Doing our best and return what's found");
            }

            return objectList;
        }

        private string GetMapName()
        {
            var mapName = "";
            try
            {
                var world = _m.GetPtr(ProcessBase + WorldOffset.ToString("x16"));
                var strCode = (UIntPtr)((ulong)world + WorldInfoOffset);
                mapName = _m.ReadUTF16(_m.GetCodeRepresentation(_m.GetPtr(_m.GetCodeRepresentation(strCode))));
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error during reading map name. Returning.");
            }

            return mapName;
        }

        public void OpenConnection()
        {
            StatusChanged?.Invoke(this, new TSWConnectionChangedEvent(TSWConnectorStatus.NotConnected));
            _enumeratingTimer.Start();
        }

        public void CloseConnection()
        {
            try
            {
                _tswControlLoop.Stop();
                _calibrationChecker.Stop();
                _trainEnumerator.Stop();
                LockClearSetCurrentLocomotive();
                _foundLocomotives.Clear();
                _gameSessionChecker.Stop();
                _m.CloseProcess();
                _mOpened = false;
                StatusChanged?.Invoke(this, new TSWConnectionChangedEvent(TSWConnectorStatus.NotConnected));
            }
            catch (Exception e)
            {
                Logger.Fatal(e);
            }
        }

        private void LockClearSetCurrentLocomotive(ILocomotive newLocomotive = null)
        {
            lock (_currentLocomotiveLock)
            {
                _currentLocomotive?.Close();
                _currentLocomotive = null;
                if (newLocomotive != null)
                {
                    _currentLocomotive = newLocomotive;
                }
                LocomotiveChanged?.Invoke(this, new TSWActiveLocomotiveChangedEvent(_currentLocomotive?.Name ?? "-"));
            }
        }

        private Process FindTSWProcess()
        {
            var processes = Process.GetProcessesByName(ProcessName);
            if (processes.Length > 0)
            {
                return processes.First();
            }

            return null;
        }

        public EventHandler<TSWConnectionChangedEvent> StatusChanged;
        public EventHandler<TSWMapChangedEvent> MapChanged;
        public EventHandler<TSWActiveLocomotiveChangedEvent> LocomotiveChanged;

        private ILocomotive _currentLocomotive;
        private readonly Dictionary<UIntPtr, ILocomotive> _foundLocomotives;
        private string _currentMap;
        private Process _currentProcess;
        private readonly Mem _m;
        private bool _mOpened;
        private bool _inSession;
        private readonly Timer _enumeratingTimer;
        private readonly Timer _gameSessionChecker;
        private readonly Timer _trainEnumerator;
        private readonly RailDriverConnector _rd;
        private readonly Timer _calibrationChecker;
        private readonly Timer _tswControlLoop;
        private object _currentLocomotiveLock;
        private object _locomotivesLock;
    }
}
