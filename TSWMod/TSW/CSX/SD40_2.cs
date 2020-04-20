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
    class SD40_2 : ILocomotive
    {
        public const string NamePartial = "SD40-2";
        private readonly IDictionary<int, InputHelpers.VirtualKeyStates> DefaultKeyMappings = new Dictionary<int, InputHelpers.VirtualKeyStates>
        {
            { 36, InputHelpers.VirtualKeyStates.VK_BACK },  // Emergency brake
            { 37, InputHelpers.VirtualKeyStates.VK_BACK },
            { 38, InputHelpers.VirtualKeyStates.VK_Q },
            { 39, InputHelpers.VirtualKeyStates.VK_X },
            { 40, InputHelpers.VirtualKeyStates.VK_P },
            { 41, InputHelpers.VirtualKeyStates.VK_B },
            { 42, InputHelpers.VirtualKeyStates.VK_SPACE },   // Horn
            { 43, InputHelpers.VirtualKeyStates.VK_SPACE },
        };

        public SD40_2(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            _m = m;
            _basePtr = basePtr;
            _hornButton = new HornButton(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x07D0)));
            _throttle = new EightNotchThrottle(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0920)));
            _reverser = new PlusMinusReverser(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0918)));
            _independentBrake = new SD40_2IndependentBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x08F8)));
            _autoBrake = new SD40_2AutoBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0908)));
            _dynamicBrake = new SD40_2DynamicBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0910)));
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
            _throttle.OnControlLoop(state.ThrottleTranslated);
            _reverser.OnControlLoop(state.ReverserTranslated);
            _independentBrake.OnControlLoop(state.IndependentBrake);
            _autoBrake.OnControlLoop(state.AutoBrakeTranslated);
            _dynamicBrake.OnControlLoop(state.DynamicBrakeTranslated);
        }

        public void Close()
        {
        }

        public void SetConfiguration(JObject config)
        {
        }

        public IDictionary<int, InputHelpers.VirtualKeyStates> GetButtonMappings()
        {
            return DefaultKeyMappings;
        }

        public JObject GetConfiguration()
        {
            return new JObject();
        }

        public string Name => "SD40-2";

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
