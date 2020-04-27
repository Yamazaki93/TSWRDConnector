using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;
using TSWMod.TSW.CSX;

namespace TSWMod.TSW.NEC
{
    class ACS_64 : ILocomotive
    {
        public const string NamePartial = "AMTK_ACS64";
        public const string MasterControllerConfigKey = "MasterControlConfiguration";
        public const string MasterControllerFollowTSW = "TSW";
        public const string MasterControllerFollowRD = "RD";

        private IDictionary<int, InputHelpers.VKCodes[]> DefaultKeyMappings =>
            new Dictionary<int, InputHelpers.VKCodes[]>
            {
                {34, KeyboardLayoutManager.Current.ReverserIncrease}, // Reverser
                {35, KeyboardLayoutManager.Current.ReverserDecrease},
                {36, KeyboardLayoutManager.Current.EmergencyBrake}, // Emergency brake
                {37, KeyboardLayoutManager.Current.EmergencyBrake},
                {38, KeyboardLayoutManager.Current.AlerterReset},
                {39, KeyboardLayoutManager.Current.Sand},
                {40, KeyboardLayoutManager.Current.PantographRaise},
                {41,  KeyboardLayoutManager.Current.Bell},
                {42, KeyboardLayoutManager.Current.Horn1}, // Horn
                {43, KeyboardLayoutManager.Current.Horn2},
            };


        public ACS_64(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            _m = m;
            _basePtr = basePtr;
            _config = new JObject
            {
                {MasterControllerConfigKey, MasterControllerFollowTSW} // TSW: follow TSW model, RD: follow RD label
            };
            _hornLeverF = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0D70)));
            _hornButtonLowF = new HornButton(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0590)));
            _hornButtonHighF = new HornButton(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0598)));

            _masterControlF = new ACSMasterController(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0CD0)));
            _independentBrakeF = new AC4400CWIndependentBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0D40)));
            _autoBrakeF = new ACS_64AutoBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0D30)));

            _hornLeverB = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0D68)));
            _hornButtonLowB = new HornButton(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0580)));
            _hornButtonHighB = new HornButton(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0588)));

            _masterControlB = new ACSMasterController(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0CC8)));
            _independentBrakeB = new AC4400CWIndependentBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0D38)));
            _autoBrakeB = new ACS_64AutoBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0D28)));
        }

        public bool CheckPlayerCalibration()
        {
            if (!_hornLeverF.CurrentValue.AlmostEquals(0) || _hornButtonLowF.CurrentValue || _hornButtonHighF.CurrentValue)  // ACS horn 0 is neutral
            {
                _hornUse += 1;
                _rearCab = false;
            }

            if (!_hornLeverB.CurrentValue.AlmostEquals(0) || _hornButtonLowB.CurrentValue || _hornButtonHighB.CurrentValue)
            {
                _hornUse += 1;
                _rearCab = true;
            }

            if (_hornUse >= 3)
            {
                return true;
            }

            return false;
        }

        public void OnControlLoop(RailDriverLeverState state, int[] pressedButtons)
        {
            var master = _masterControlF;
            var trainBrake = _autoBrakeF;
            var independentBrake = _independentBrakeF;
            if (_rearCab)
            {
                master = _masterControlB;
                trainBrake = _autoBrakeB;
                independentBrake = _independentBrakeB;
            }

            if (!state.BailOff)
            {
                independentBrake.DisengageBailOffIfNeeded();
            }
            if (_config[MasterControllerConfigKey].Value<string>().Equals(MasterControllerFollowRD))
            {
                master.OnControlLoop(_masterControlF.TranslateCombinedValue(state.ThrottleTranslated,
                    state.DynamicBrakeTranslated));
            }
            else if (_config[MasterControllerConfigKey].Value<string>().Equals(MasterControllerFollowTSW))
            {
                // in TSW, ACS-64 lever forward is throttle
                master.OnControlLoop(_masterControlF.TranslateCombinedValue(state.DynamicBrakeTranslated,
                    state.ThrottleTranslated));
            }

            independentBrake.OnControlLoop(state.IndependentBrake);
            trainBrake.OnControlLoop(state.AutoBrakeTranslated);

            if (state.BailOff)
            {
                independentBrake.EngageBailOffIfNeeded();
            }
        }

        public void Close()
        {
        }

        public void SetConfiguration(JObject config)
        {
            _config = config;
        }

        public IDictionary<int, InputHelpers.VKCodes[]> GetButtonMappings()
        {
            return DefaultKeyMappings;
        }

        public JObject GetConfiguration()
        {
            return _config;
        }

        public string Name => "ACS-64";

        private bool _rearCab;
        private int _hornUse;
        private JObject _config;
        private readonly TSWLever _hornLeverF;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private readonly AC4400CWIndependentBrake _independentBrakeF;
        private readonly HornButton _hornButtonLowF;
        private readonly ACSMasterController _masterControlF;
        private readonly HornButton _hornButtonHighF;
        private readonly ACS_64AutoBrake _autoBrakeF;
        private readonly HornLever _hornLeverB;
        private readonly HornButton _hornButtonLowB;
        private readonly HornButton _hornButtonHighB;
        private readonly ACSMasterController _masterControlB;
        private readonly AC4400CWIndependentBrake _independentBrakeB;
        private readonly ACS_64AutoBrake _autoBrakeB;
    }
}
