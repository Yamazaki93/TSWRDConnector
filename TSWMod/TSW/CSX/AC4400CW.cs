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
            _lightFR = new GenericLightSelector(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0A70)));
            _wiper = new ContinuousWiperLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x08E0)));
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
            _lightFR.OnControlLoop(state.LightTranslated);
            _wiper.OnControlLoop(state.WiperTranslated);
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

        public string Name => "AC4400CW";

        private int _hornUse;
        private readonly TSWButton _hornButton;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private readonly AC4400CWThrottle _throttle;
        private readonly PlusMinusReverser _reverser;
        private readonly AC4400CWIndependentBrake _independentBrake;
        private readonly AC4400CWAutoBrake _autoBrake;
        private readonly GenericLightSelector _lightFR;
        private readonly ContinuousWiperLever _wiper;
    }
}
