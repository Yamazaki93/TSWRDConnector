using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using PIEHid64Net;

namespace TSWMod.RailDriver
{
    class RailDriverConnector : PIEDataHandler, PIEErrorHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();        
        
        // Credit: https://gist.github.com/rwaldron/0dd696800d2a09786ec2
        private static readonly Dictionary<char, byte> LEDLookup = new Dictionary<char, byte>
        {
            { ' ', 0x00 },   //0x20 ' '
            {'!', 0x86 },   //0x21 '!'
            {'"', 0x22},   //0x22 '"'
            {'#', 0x7E},   //0x23 '#' ?
            {'$', 0x2D},   //0x24 '$' ?
            {'%', 0xD2},   //0x25 '%' ?
            {'&', 0x7B},   //0x26 '&' ?
            {'\'', 0x20},   //0x27 '''
            {'(', 0x39},   //0x28 '('
            {')', 0x0F},   //0x29 ')'
            {'*', 0x63},   //0x2A '*' ?
            {'+', 0x00},   //0x2B '+' ?
            {',', 0x10},   //0x2C ','
            {'-', 0x40},   //0x2D '-'
            {'.', 0x80},   //0x2E '.'
            {'/', 0x52},   //0x2F '/' ?
            //3,-3F
            {'0', 0x3F},   //0x30 '0'
            {'1', 0x06},   //0x31 '1'
            {'2', 0x5B},   //0x32 '2'
            {'3', 0x4F},   //0x33 '3'
            {'4', 0x66},   //0x34 '4'
            {'5', 0x6D},   //0x35 '5'
            {'6', 0x7D},   //0x36 '6'
            {'7', 0x07},   //0x37 '7'
            {'8', 0x7F},   //0x38 '8'
            {'9', 0x6F},   //0x39 '9'
            {':', 0x09},   //0x3A ':' ?
            {';', 0x0D},   //0x3B ';' ?
            {'<', 0x58},   //0x3C '<' ?
            {'=', 0x48},   //0x3D '='
            {'>', 0x4C},   //0x3E '>' ?
            {'?', 0xD3},   //0x3F '?' ?
            //4,-4F  
            {'@', 0x5F},   //0x40 '@' ?
            {'A', 0x77},   //0x41 'A'
            {'B', 0x7C},   //0x42 'B'
            {'C', 0x39},   //0x43 'C'
            {'D', 0x5E},   //0x44 'D'
            {'E', 0x79},   //0x45 'E'
            {'F', 0x71},   //0x46 'F'
            {'G', 0x3D},   //0x47 'G'
            {'H', 0x76},   //0x48 'H'
            {'I', 0x30},   //0x49 'I'
            {'J', 0x1E},   //0x4A 'J'
            {'K', 0x75},   //0x4B 'K' ?
            {'L', 0x38},   //0x4C 'L'
            {'M', 0x37},   //0x4D 'M'
            {'N', 0x54},   //0x4E 'N'
            {'O', 0x3F},   //0x4F 'O'
            //5,-5F  
            {'P', 0x73},   //0x50 'P'
            {'Q', 0x67},   //0x51 'Q' ?
            {'R', 0x50},   //0x52 'R'
            {'S', 0x6D},   //0x53 'S'
            {'T', 0x78},   //0x54 'T'
            {'U', 0x3E},   //0x55 'U'
            {'V', 0x1C},   //0x56 'V' ? (u)
            {'W', 0x2A},   //0x57 'W' ?
            {'X', 0x76},   //0x58 'X' ? (like H)
            {'Y', 0x6E},   //0x59 'Y'
            {'Z', 0x5B},   //0x5A 'Z' ?
            {'[', 0x39},   //0x5B '['
            {'\\', 0x5C},   //0x5C '\' ?
            {']', 0x0F},   //0x5D ']'
            {'^', 0x23},   //0x5E '^' ?
            {'_', 0x08},   //0x5F '_'
            //6,-6F  
            {'`', 0x02},   //0x60 '`' ?
            {'a', 0x77},   //0x61 'a'
            {'b', 0x7C},   //0x62 'b'
            {'c', 0x58},   //0x63 'c'
            {'d', 0x5E},   //0x66 'd'
            {'e', 0x79},   //0x65 'e'
            {'f', 0x71},   //0x66 'f'
            {'g', 0x3D},   //0x67 'g'
            {'h', 0x74},   //0x68 'h'
            {'i', 0x10},   //0x69 'i'
            {'j', 0x1E},   //0x6A 'j'
            {'k', 0x75},   //0x6B 'k' ?
            {'l', 0x38},   //0x6C 'l'
            {'m', 0x37},   //0x6D 'm'
            {'n', 0x54},   //0x6E 'n'
            {'o', 0x5C},   //0x6F 'o'
            //7,-7F  
            {'p', 0x73},   //0x70 'p'
            {'q', 0x67},   //0x71 'q'
            {'r', 0x50},   //0x72 'r'
            {'s', 0x6D},   //0x73 's'
            {'t', 0x78},   //0x74 't'
            {'u', 0x3E},   //0x77 'u' ? (like U)
            {'v', 0x1C},   //0x76 'v' ? (like u)
            {'w', 0x2A},   //0x77 'w' ?
            {'x', 0x76},   //0x78 'x' ? (like H)
            {'y', 0x6E},   //0x79 'y'
            {'z', 0x5B},   //0x7A 'z' ?
            {'{', 0x46},   //0x7B '{' ?
            {'|', 0x30},   //0x7C '|'
            {'}', 0x70},   //0x7D '}' ?
            {'~', 0x01},   //0x7E '~' ?
        };

        public RailDriverConnector()
        {
            _enumeratingTimer = new Timer(500) {AutoReset = true};
            _enumeratingTimer.Elapsed += EnumeratingTimerOnElapsed;
            _ledWriteTimer = new Timer(500) {AutoReset = true};
            _ledWriteTimer.Elapsed += LEDWriteTimerOnElapsed;
            _persistentMessage = "rd";
            _ledWriteTimer.Start();
            CurrentLeverState = RailDriverLeverState.Invalid;
        }

        private void LEDWriteTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (_flashingStatus)
            {
                var message = TrimStatusMessageCharToWrite();
                WriteLED(message);
                if (string.IsNullOrEmpty(message))
                {
                    _flashingStatus = false;

                }
            }

            else
            {
                WriteLED(_persistentMessage);
            }
        }

        public RailDriverLeverState CurrentLeverState { get; private set; }

        public void OpenConnection()
        {
            _enumeratingTimer.Start();
            Logger.Info("RDConnector starting search for RD devices...");
        }

        public void CloseConnection()
        {
            try
            {
                _currentDevice?.CloseInterface();
            }
            catch (Exception e)
            {
                Logger.Fatal(e);
            }
            finally
            {
                Connected?.Invoke(this, new RailDriverConnectionChangedEvent(false));
            }
        }

        private void EnumeratingTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            var devices = PIEDevice.EnumeratePIE();
            if (devices.Length > 0)
            {
                Logger.Info($"Found {devices.Length} PI Devices");
                foreach (var device in devices)
                {
                    if (device.HidUsagePage == 0xc && device.Pid == 210)   // 210: rail driver device
                    {
                        _enumeratingTimer.Stop();
                        _currentDevice = device;
                        device.SetupInterface();
                        _currentDevice.SetDataCallback(this);
                        _currentDevice.SetErrorCallback(this);
                        _currentDevice.callNever = false;
                        _writeData = new byte[_currentDevice.WriteLength];
                        WriteLED("rd");
                        Logger.Info($"RD device connected. Version: {_currentDevice.Version}");
                        Connected?.Invoke(this, new RailDriverConnectionChangedEvent(true));
                        return;
                    }
                }
            }
        }

        public void SetPersistentMessage(string message)
        {
            _persistentMessage = message;
        }

        public void FlashStatus(string message)
        {
            _ledWriteTimer.Stop();
            _flashingStatus = true;
            _statusMessageLeftToWrite = message;
            var firstMessage = TrimStatusMessageCharToWrite();
            WriteLED(firstMessage);
            _ledWriteTimer.Start();
        }

        private string TrimStatusMessageCharToWrite()
        {
            if (_statusMessageLeftToWrite.Length > 3)
            {
                var message = _statusMessageLeftToWrite.Substring(0, 3);
                _statusMessageLeftToWrite = _statusMessageLeftToWrite.Remove(0, 3);
                return message;
            }
            else
            {
                var message = _statusMessageLeftToWrite;
                _statusMessageLeftToWrite = "";
                return message;
            }
        }

        public bool LoadCalibrationFile(string filePath)
        {
            try
            {
                _reverser = null;
                _throttle = null;
                _autoBrake = null;
                _independentBrake = null;
                var text = File.ReadAllLines(filePath);
                // parse RD calibration
                if (text[0] != "Calibration")
                {
                    throw new NotSupportedException("Calibration file malformed");
                }

                ParseIntFromCalibrationLine(text[16], out var reverserMin);
                ParseIntFromCalibrationLine(text[7], out var reverserMax);
                ParseIntFromCalibrationLine(text[11], out var neutralMin);
                ParseIntFromCalibrationLine(text[12], out var neutralMax);
                _reverser = new RDReverser(reverserMin, reverserMax, neutralMin, neutralMax);

                ParseIntFromCalibrationLine(text[34], out var throttleMin);
                ParseIntFromCalibrationLine(text[25], out var throttleMax);
                ParseIntFromCalibrationLine(text[29], out var idleMin);
                ParseIntFromCalibrationLine(text[30], out var idleMax);
                ParseIntFromCalibrationLine(text[39], out var dynSetupMin);
                ParseIntFromCalibrationLine(text[40], out var dynSetupMax);
                _throttle = new RDThrottleDynamicBrake(throttleMin, throttleMax, idleMin, idleMax, dynSetupMin, dynSetupMax);


                ParseIntFromCalibrationLine(text[57], out var autoBrakeMin);
                ParseIntFromCalibrationLine(text[48], out var autoBrakeMax);
                ParseIntFromCalibrationLine(text[52], out var csMin);
                ParseIntFromCalibrationLine(text[53], out var csMax);
                _autoBrake = new RDAutoBrake(autoBrakeMin, autoBrakeMax, csMin, csMax);

                ParseIntFromCalibrationLine(text[75], out var independentBrakeMin);
                ParseIntFromCalibrationLine(text[66], out var independentBrakeMax);
                ParseIntFromCalibrationLine(text[70], out var bailOffEngagedMin);
                ParseIntFromCalibrationLine(text[85], out var bailOffDisengagedMin);
                _independentBrake = new RDIndependentBrake(independentBrakeMin, independentBrakeMax, bailOffEngagedMin, bailOffDisengagedMin);

                ParseIntFromCalibrationLine(text[98], out var wiperMin);
                ParseIntFromCalibrationLine(text[103], out var wiperMiddle);
                ParseIntFromCalibrationLine(text[109], out var wiperMax);
                _wiper = new RDWiper(wiperMin, wiperMax, wiperMiddle);

                ParseIntFromCalibrationLine(text[113], out var lightMin);
                ParseIntFromCalibrationLine(text[118], out var lightMiddle);
                ParseIntFromCalibrationLine(text[124], out var lightMax);
                _light = new RDLight(lightMin, lightMax, lightMiddle);

                Logger.Info("RD calibration loaded");
                Logger.Info($"Reverser {_reverser.Min} - {_reverser.Max}");
                Logger.Info($"ThrottleDynBrake {_throttle.Min} - {_throttle.Max}");
                Logger.Info($"AutBrake {_autoBrake.Min} - {_autoBrake.Max}");
                Logger.Info($"IndependentBrake {_independentBrake.Min} - {_independentBrake.Max}");
                Logger.Info($"Wiper {_wiper.Min} - {_wiper.Max}");
                Logger.Info($"Light {_light.Min} - {_light.Max}");
            }
            catch (Exception e)
            {
                Logger.Fatal(e, $"Error during loading calibration");
                return false;
            }

            return true;
        }

        public void HandlePIEHidData(byte[] data, PIEDevice sourceDevice, int error)
        {
            if (_reverser == null || _autoBrake == null || _throttle == null || _independentBrake == null)
            {
                return;
            }
            if (sourceDevice == _currentDevice)
            {
                var changed = false;
                if (_reverser.CurrentValue != data[1])
                {
                    changed = true;
                    _reverser.CurrentValue = data[1];
                }
                if (_throttle.CurrentValue != data[2])
                {
                    changed = true;
                    _throttle.CurrentValue = data[2];
                }
                if (_autoBrake.CurrentValue != data[3])
                {
                    changed = true;
                    _autoBrake.CurrentValue = data[3];
                }
                if (_independentBrake.CurrentValue != data[4])
                {
                    changed = true;
                    _independentBrake.CurrentValue = data[4];
                }

                if (_independentBrake.BailOffValue != data[5])
                {
                    changed = true;
                    _independentBrake.BailOffValue = data[5];
                }
                if (_wiper.CurrentValue != data[6])
                {
                    changed = true;
                    _wiper.CurrentValue = data[6];
                }
                if (_light.CurrentValue != data[7])
                {
                    changed = true;
                    _light.CurrentValue = data[7];
                }

                if (changed)
                {
                    Logger.Trace($"RD Lever Changed " +
                                 $"{_reverser.CurrentValueScaled}/{_reverser.TranslatedValue} - {_throttle.CurrentValueScaled}/{_throttle.TranslatedThrottleValue}/{_throttle.TranslatedDynamicBrakeValue}" +
                                 $" - {_autoBrake.CurrentValueScaled}/{_autoBrake.TranslatedValue} - {_independentBrake.CurrentValueScaled}/BO:{_independentBrake.BailOffEngaged} - " +
                                 $"{_wiper.CurrentValueScaled}/{_wiper.GetTranslatedValue()} - {_light.CurrentValueScaled}/{_light.GetTranslatedValue()}");
                    var state = new RailDriverLeverState(
                        _reverser.CurrentValueScaled, 
                        _throttle.CurrentValueScaled, 
                        _autoBrake.CurrentValueScaled, 
                        _independentBrake.CurrentValueScaled, 
                        _wiper.CurrentValueScaled, 
                        _light.CurrentValueScaled,
                        _reverser.TranslatedValue,
                        _throttle.TranslatedThrottleValue,
                        _throttle.TranslatedDynamicBrakeValue,
                        _autoBrake.TranslatedValue,
                        _independentBrake.BailOffEngaged,
                        _light.GetTranslatedValue(),
                        _wiper.GetTranslatedValue());
                    CurrentLeverState = state;
                    LeverChanged?.Invoke(this, new RailDriverLeverUpdatedEvent(state));
                }

                if (CurrentLeverState == RailDriverLeverState.Invalid)
                {
                    CurrentLeverState = new RailDriverLeverState(
                            _reverser.CurrentValueScaled,
                            _throttle.CurrentValueScaled,
                            _autoBrake.CurrentValueScaled,
                            _independentBrake.CurrentValueScaled,
                            _wiper.CurrentValueScaled,
                            _light.CurrentValueScaled,
                            _reverser.TranslatedValue,
                            _throttle.TranslatedThrottleValue,
                            _throttle.TranslatedDynamicBrakeValue,
                            _autoBrake.TranslatedValue,
                            _independentBrake.BailOffEngaged,
                            _light.GetTranslatedValue(),
                            _wiper.GetTranslatedValue());
                }

                var buttonValue = BitConverter.ToUInt64(data.Skip(8).Take(6).Append((byte)0).Append((byte)0).ToArray(), 0);
                
                for (var i = 0; i < 44; i++)
                {
                    if (IsBitSet(buttonValue, i) && !IsBitSet(_pressedButtons, i))
                    {
                        Logger.Trace($"Button Pressed {BitConverter.ToString(BitConverter.GetBytes(buttonValue))} ({buttonValue})");
                        _pressedButtons = SetBit(_pressedButtons, i);
                        ButtonPressed?.Invoke(this, new RailDriverButtonEvent(i));
                    }
                    else if (!IsBitSet(buttonValue, i) && IsBitSet(_pressedButtons, i))
                    {
                        Logger.Trace($"Button Released {BitConverter.ToString(BitConverter.GetBytes(buttonValue))} ({buttonValue})");
                        _pressedButtons = ClearBit(_pressedButtons, i);
                        ButtonReleased?.Invoke(this, new RailDriverButtonEvent(i));
                    }
                }
            }
        }

        public void HandlePIEHidError(PIEDevice sourceDevices, long error)
        {
            if (sourceDevices == _currentDevice)
            {
                CloseConnection();
                Logger.Error($"PIE Error, {error}. Disconnecting from RD. Starting to reconnect...");
                _enumeratingTimer.Start();
            }
        }

        public event EventHandler<RailDriverConnectionChangedEvent> Connected;
        public event EventHandler<RailDriverLeverUpdatedEvent> LeverChanged;
        public event EventHandler<RailDriverButtonEvent> ButtonPressed;
        public event EventHandler<RailDriverButtonEvent> ButtonReleased;

        private bool IsBitSet(ulong b, int pos)
        {
            return (b & ((ulong)1 << pos)) != 0;
        }

        private ulong SetBit(ulong b, int pos)
        {
            return b |= (ulong)1 << pos;
        }

        private ulong ClearBit(ulong b, int pos)
        {
            return b &= ~((ulong)1 << pos);
        }
        private bool ParseIntFromCalibrationLine(string line, out int number)
        {
            var strings = line.Split(' ');
            var success = int.TryParse(strings.Last(), out number);
            if (!success)
            {
                Logger.Error($"Error during parsing calibration line, the line is {line}");
                return false;
            }

            return true;
        }

        private bool WriteLED(string text)
        {
            if (_currentDevice == null || _writeData == null)
                return false;
            //write to the LED Segments
            text = text.Length > 3 ? text.Substring(0, 3) : text.PadRight(3);
            for (var j = 0; j < _currentDevice.WriteLength; j++)
            {
                _writeData[j] = 0;
            }

            text = new string(text.Reverse().ToArray());
            _writeData[1] = 134;
            _writeData[2] = ConvertCharToLED(text[0]);
            _writeData[3] = ConvertCharToLED(text[1]);
            _writeData[4] = ConvertCharToLED(text[2]);
            int result = 404;
            while (result == 404)
            {
                result = _currentDevice.WriteData(_writeData);
            }
            if (result != 0)
            {
                Logger.Warn($"RD write failed, result {result}");
                return false;
            }

            return true;
        }

        private byte ConvertCharToLED(char c)
        {
            return LEDLookup.ContainsKey(c) ? LEDLookup[c] : LEDLookup[' '];
        }

        private ulong _pressedButtons;
        private PIEDevice _currentDevice;
        private RDReverser _reverser;
        private RDThrottleDynamicBrake _throttle;
        private RDAutoBrake _autoBrake;
        private RDIndependentBrake _independentBrake;
        private RDWiper _wiper;
        private RDLight _light;
        private byte[] _writeData;
        private bool _flashingStatus;
        private string _statusMessageLeftToWrite;
        private string _persistentMessage;
        private readonly Timer _enumeratingTimer;
        private readonly Timer _ledWriteTimer;
    }
}
