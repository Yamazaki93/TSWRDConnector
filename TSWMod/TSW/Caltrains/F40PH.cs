using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;
using TSWMod.TSW.CSX;

namespace TSWMod.TSW.Caltrains
{
    class F40PH : ILocomotive
    {
        public const string NamePartial = "F40PH-2CAT";
        private IDictionary<int, InputHelpers.VKCodes[]> DefaultKeyMappings =>
            new Dictionary<int, InputHelpers.VKCodes[]>
            {
                {36, KeyboardLayoutManager.Current.EmergencyBrake}, // Emergency brake
                {37, KeyboardLayoutManager.Current.EmergencyBrake},
                {38, KeyboardLayoutManager.Current.AlerterReset},
                {39, KeyboardLayoutManager.Current.Sand},
                {40, KeyboardLayoutManager.Current.PantographRaise},
                {41,  KeyboardLayoutManager.Current.Bell},
                {42, KeyboardLayoutManager.Current.Horn1}, // Horn
                {43, KeyboardLayoutManager.Current.Horn1},
            };

        public F40PH(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            _m = m;
            _basePtr = basePtr;
            _hornButton = new HornButton(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0698)));
            _throttle = new EightNotchThrottle(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x06E0)));
            _reverser = new PlusMinusReverser(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x6F0)));
            _independentBrake = new F40PHIndependentBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x6C8)));
            _autoBrake = new F40PHAutomaticBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x06D0)));
            _dynamicBrake = new F40PHDynamicBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x06D8)));
        }

        public bool CheckPlayerCalibration()
        {
            if (_hornButton.CurrentValue)
            {
                _hornUse += 1;
            }

            if (_hornUse >= 2)
            {
                return true;
            }

            return false;
        }

        public void OnControlLoop(RailDriverLeverState state, int[] pressedButtons)
        {
            if (!state.BailOff)
            {
                _independentBrake.DisengageBailOffIfNeeded();
            }
            _throttle.OnControlLoop(state.ThrottleTranslated);
            _reverser.OnControlLoop(state.ReverserTranslated);
            _independentBrake.OnControlLoop(state.IndependentBrake);
            _autoBrake.OnControlLoop(state.AutoBrakeTranslated);
            _dynamicBrake.OnControlLoop(state.DynamicBrakeTranslated);
            if (state.BailOff)
            {
                _independentBrake.EngageBailOffIfNeeded();
            }
        }

        public void Close()
        {
        }

        public void SetConfiguration(JObject config)
        {
        }

        public IDictionary<int, InputHelpers.VKCodes[]> GetButtonMappings()
        {
            return DefaultKeyMappings;
        }

        public JObject GetConfiguration()
        {
            return null;
        }

        public string Name => "F40PH-2";

        private int _hornUse;
        private readonly TSWButton _hornButton;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private readonly EightNotchThrottle _throttle;
        private readonly PlusMinusReverser _reverser;
        private readonly F40PHIndependentBrake _independentBrake;
        private readonly F40PHAutomaticBrake _autoBrake;
        private readonly F40PHDynamicBrake _dynamicBrake;
    }
}
