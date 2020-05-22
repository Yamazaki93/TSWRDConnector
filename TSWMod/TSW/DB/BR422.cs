using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;

namespace TSWMod.TSW.DB
{
    class BR422 : ILocomotive
    {
        public const string NamePartial = "DB_BR422_FrontCabUnit";
        public const string NamePartial2 = "DB_BR422_RearCabUnit";
        public const string MasterControllerConfigKey = "MasterControlConfiguration";
        public const string MasterControllerFollowTSW = "TSW";
        public const string MasterControllerFollowRD = "RD";
        private IDictionary<int, InputHelpers.VKCodes[]> DefaultKeyMappings =>
            new Dictionary<int, InputHelpers.VKCodes[]>
            {
                {2, KeyboardLayoutManager.Current.PZBAck}, // PZB Ack
                {3, KeyboardLayoutManager.Current.PZBRelease}, // PZB Release
                {4, KeyboardLayoutManager.Current.PZBOverride}, // PZB Override
                {5, KeyboardLayoutManager.Current.CruiseControlToggle},
                {34, KeyboardLayoutManager.Current.ReverserIncrease}, // Reverser
                {35, KeyboardLayoutManager.Current.ReverserDecrease},
                {36, KeyboardLayoutManager.Current.EmergencyBrake}, // Emergency brake
                {37, KeyboardLayoutManager.Current.EmergencyBrake},
                {38, KeyboardLayoutManager.Current.AlerterReset},
                {39, KeyboardLayoutManager.Current.Sand},
                {40, KeyboardLayoutManager.Current.PantographRaise},
                {42, KeyboardLayoutManager.Current.Horn1}, // Horn
                {43, KeyboardLayoutManager.Current.Horn2},
            };

        public BR422(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            _config = new JObject
            {
                {MasterControllerConfigKey, MasterControllerFollowTSW} // TSW: follow TSW model, RD: follow RD label
            };
            _m = m;
            _basePtr = basePtr;
            _hornLeverFR = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x08A0)));
            _masterController = new BR422_MasterController(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x08E8)));

        }

        public bool CheckPlayerCalibration()
        {
            if (!_hornLeverFR.CurrentValue.AlmostEquals(0.5f))
            {
                _hornUse += 1;
            }

            if (_hornUse >= 3)
            {
                return true;
            }

            return false;
        }

        public void OnControlLoop(RailDriverLeverState state, int[] pressedButtons)
        {
            if (_config[MasterControllerConfigKey].Value<string>().Equals(MasterControllerFollowRD))
            {
                _masterController.OnControlLoop(_masterController.TranslateCombinedValue(state.ThrottleTranslated, state.DynamicBrakeTranslated));
            }
            else
            {
                _masterController.OnControlLoop(_masterController.TranslateCombinedValue(state.DynamicBrakeTranslated, state.ThrottleTranslated)); // flip throttle/brake for TSW
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

        public string Name => "DB BR422";

        private int _hornUse;
        private readonly TSWLever _hornLeverFR;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private JObject _config;
        private readonly BR422_MasterController _masterController;
    }
}
