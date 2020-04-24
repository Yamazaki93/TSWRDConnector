using System;
using System.Collections.Generic;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;

namespace TSWMod.TSW.CSX
{
    class AC4400CW : ILocomotive
    {
        public const string NamePartial = "AC4400CW";
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

        public AC4400CW(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            _m = m;
            _basePtr = basePtr;
            _hornButton = new HornButton(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0948)));
            _throttle = new AC4400CWThrottle(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0A98)));
            _reverser = new PlusMinusReverser(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0AA0)));
            _independentBrake = new AC4400CWIndependentBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0A90)));
            _autoBrake = new AC4400CWAutoBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0A88)));
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
            _throttle.OnControlLoop(_throttle.TranslateCombinedValue(state.ThrottleTranslated, state.DynamicBrakeTranslated));
            _reverser.OnControlLoop(state.ReverserTranslated);
            _independentBrake.OnControlLoop(state.IndependentBrake);
            _autoBrake.OnControlLoop(state.AutoBrakeTranslated);
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

        public string Name => "AC4400CW";

        private int _hornUse;
        private readonly TSWButton _hornButton;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private readonly AC4400CWThrottle _throttle;
        private readonly PlusMinusReverser _reverser;
        private readonly AC4400CWIndependentBrake _independentBrake;
        private readonly AC4400CWAutoBrake _autoBrake;
    }
}
