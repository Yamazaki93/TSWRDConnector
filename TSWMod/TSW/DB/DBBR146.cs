using System;
using System.Collections.Generic;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;

namespace TSWMod.TSW.DB
{
    class DB146_2 : ILocomotive
    {
        public const string NamePartial = "DB_BR146-2";

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

        public DB146_2(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            _m = m;
            _basePtr = basePtr;
            _hornLeverFR = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x798)));
            _throttleLeverF = new DBThrottleLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0A60)));
            _afbLeverF = new DBAFBLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0A58)));
            _trainBrakeLeverF = new DBTrainBrakeLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0A50)));
            _directBrakeLeverF = new DBDirectBrakeLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0990)));
            _lightF = new DBLightSelector(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0A30)));
            _wiperF = new DBWiperSelector(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x06E0)));

            _hornLeverBR = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0790)));
            _throttleLeverB = new DBThrottleLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0890)));
            _afbLeverB = new DBAFBLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0888)));
            _trainBrakeLeverB = new DBTrainBrakeLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0880)));
            _directBrakeLeverB = new DBDirectBrakeLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0768)));
            _lightB = new DBLightSelector(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0870)));
            _wiperB = new DBWiperSelector(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x06D8)));
        }

        public bool CheckPlayerCalibration()
        {
            if (!_hornLeverFR.CurrentValue.AlmostEquals(0.5f))
            {
                _hornUse += 1;
                _rearCab = false;
            }

            if (!_hornLeverBR.CurrentValue.AlmostEquals(0.5f))
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
            if (!_rearCab)
            {
                _afbLeverF.OnControlLoop(state.Reverser);
                _throttleLeverF.OnControlLoop(state.Throttle);
                _trainBrakeLeverF.OnControlLoop(state.AutoBrake);
                _directBrakeLeverF.OnControlLoop(state.IndependentBrake);
                _lightF.OnControlLoop(state.LightTranslated);
                _wiperF.OnControlLoop(state.WiperTranslated);
            }
            else
            {
                _afbLeverB.OnControlLoop(state.Reverser);
                _throttleLeverB.OnControlLoop(state.Throttle);
                _trainBrakeLeverB.OnControlLoop(state.AutoBrake);
                _directBrakeLeverB.OnControlLoop(state.IndependentBrake);
                _lightB.OnControlLoop(state.LightTranslated);
                _wiperB.OnControlLoop(state.WiperTranslated);
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

        public string Name => "DB BR146-2";

        private int _hornUse;
        private bool _rearCab;
        private readonly TSWLever _hornLeverFR;
        private readonly TSWLever _throttleLeverF;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private readonly DBAFBLever _afbLeverF;
        private readonly DBTrainBrakeLever _trainBrakeLeverF;
        private readonly DBDirectBrakeLever _directBrakeLeverF;
        private readonly HornLever _hornLeverBR;
        private readonly DBThrottleLever _throttleLeverB;
        private readonly DBAFBLever _afbLeverB;
        private readonly DBTrainBrakeLever _trainBrakeLeverB;
        private readonly DBDirectBrakeLever _directBrakeLeverB;
        private readonly DBLightSelector _lightF;
        private readonly DBWiperSelector _wiperF;
        private readonly DBLightSelector _lightB;
        private readonly DBWiperSelector _wiperB;
    }
}
