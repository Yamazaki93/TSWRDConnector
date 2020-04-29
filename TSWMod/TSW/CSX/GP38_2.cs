using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;

namespace TSWMod.TSW.CSX
{

    // GP38-2 is almost identical to SD40-2
    // NEC GP38-2 is the same as this one
    // SF - SJ GP38-2 is the same as this one
    class GP38_2 : ILocomotive
    {
        public const string NamePartial = "GP38-2";
        public const string SFJPartial = "SFJ_CT";
        public const string YN3NamePartial = "YN3_GP38-2";
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

        public static GP38_2 CreateCSXHeavyHaulVersion(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            return new GP38_2(m, basePtr, hWnd, 0x0938, 0x0928, 0x0930, 0x06C8, 0x0800, 0x0750);
        }

        public static GP38_2 CreateNECVersion(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            return new GP38_2(m, basePtr, hWnd, 0x0940, 0x0930, 0x0938, 0x06D0, 0x0808, 0x0758);
        }

        public static GP38_2 CreateSFJVersion(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            return new GP38_2(m, basePtr, hWnd, 0x0938, 0x0928, 0x0930, 0x06F0, 0x0800, 0x0758);
        }

        private GP38_2(Mem m, UIntPtr basePtr, IntPtr hWnd, int hornOffset, int throttleOffset, int reverserOffset, int independentBrakeOffset, int automaticBrakeOffset, int dynamicBrakeOffset)
        {
            _m = m;
            _basePtr = basePtr;
            _hornButton = new HornButton(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + hornOffset)));
            _throttle = new EightNotchThrottle(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + throttleOffset)));
            _reverser = new PlusMinusReverser(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + reverserOffset)));
            _independentBrake = new SD40_2IndependentBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + independentBrakeOffset)));
            _autoBrake = new SD40_2AutoBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + automaticBrakeOffset)));
            _dynamicBrake = new SD40_2DynamicBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + dynamicBrakeOffset)));
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

        public string Name => "GP38-2";

        private int _hornUse;
        private readonly TSWButton _hornButton;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private readonly EightNotchThrottle _throttle;
        private readonly PlusMinusReverser _reverser;
        private readonly SD40_2IndependentBrake _independentBrake;
        private readonly SD40_2AutoBrake _autoBrake;
        private readonly SD40_2DynamicBrake _dynamicBrake;
    }
}
