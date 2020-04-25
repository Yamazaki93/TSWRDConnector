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
    class GP38_2 : ILocomotive
    {
        public const string NamePartial = "GP38-2";
        public const string YN3NamePartial = "YN3_GP38-2";
        private readonly IDictionary<int, InputHelpers.VirtualKeyStates[]> DefaultKeyMappings = new Dictionary<int, InputHelpers.VirtualKeyStates[]>
        {
            { 36, new []{InputHelpers.VirtualKeyStates.VK_BACK }},  // Emergency brake
            { 37, new []{InputHelpers.VirtualKeyStates.VK_BACK }},
            { 38, new []{InputHelpers.VirtualKeyStates.VK_Q }},
            { 39, new []{InputHelpers.VirtualKeyStates.VK_X }},
            { 40, new []{InputHelpers.VirtualKeyStates.VK_P }},
            { 41, new []{InputHelpers.VirtualKeyStates.VK_B }},
            { 42, new []{InputHelpers.VirtualKeyStates.VK_SPACE }},   // Horn
            { 43, new []{InputHelpers.VirtualKeyStates.VK_SPACE }},
        };

        public GP38_2(Mem m, UIntPtr basePtr, IntPtr hWnd, bool yn3)
        {
            _m = m;
            _basePtr = basePtr;
            var yn3Offset = yn3 ? 0x08 : 0x00;
            _hornButton = new HornButton(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0938 + yn3Offset)));
            _throttle = new EightNotchThrottle(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0928 + yn3Offset)));
            _reverser = new PlusMinusReverser(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0930 + yn3Offset)));
            _independentBrake = new SD40_2IndependentBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x06C8 + yn3Offset)));
            _autoBrake = new SD40_2AutoBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0800 + yn3Offset)));
            _dynamicBrake = new SD40_2DynamicBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0750 + yn3Offset)));
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

        public IDictionary<int, InputHelpers.VirtualKeyStates[]> GetButtonMappings()
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
