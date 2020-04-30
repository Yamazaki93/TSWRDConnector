using System;
using System.Collections.Generic;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;

namespace TSWMod.TSW.DB
{
    class DB_766pbzfa : ILocomotive
    {
        public const string NamePartial = "DB_766pbzfa";
        private IDictionary<int, InputHelpers.VKCodes[]> DefaultKeyMappings =>
            new Dictionary<int, InputHelpers.VKCodes[]>
            {
                {2, KeyboardLayoutManager.Current.PZBAck}, // PZB Ack
                {3, KeyboardLayoutManager.Current.PZBRelease}, // PZB Release
                {4, KeyboardLayoutManager.Current.PZBOverride}, // PZB Override
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

        public DB_766pbzfa(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            _m = m;
            _basePtr = basePtr;
            _hornLeverFR = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0540)));
            _throttleLeverF = new DBThrottleLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x08C0)));
            _trainBrakeLeverF = new DBTrainBrakeLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0870)));
            _directBrakeLeverF = new DBDirectBrakeLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0840)));
            _light = new DBLightSelector(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0860)));
            _wiper = new GenericWiperSelector(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0830)));
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
            _throttleLeverF.OnControlLoop(state.Throttle);
            _trainBrakeLeverF.OnControlLoop(state.AutoBrake);
            _directBrakeLeverF.OnControlLoop(state.IndependentBrake);
            _light.OnControlLoop(state.LightTranslated);
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

        public string Name => "DB 766pbzfa";

        private int _hornUse;
        private readonly TSWLever _hornLeverFR;
        private readonly TSWLever _throttleLeverF;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private readonly DBTrainBrakeLever _trainBrakeLeverF;
        private readonly DBDirectBrakeLever _directBrakeLeverF;
        private readonly DBLightSelector _light;
        private readonly GenericWiperSelector _wiper;
    }
}
