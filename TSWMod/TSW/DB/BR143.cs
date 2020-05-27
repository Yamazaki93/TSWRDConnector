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
    class BR143 : ILocomotive
    {
        public const string NamePartial = "DB_BR143";
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

        public BR143(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            _m = m;
            _basePtr = basePtr;
            _hornLeverFR = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0510)));
            _throttleLeverF = new BR143AFBLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x09C8)));
            _tractionLeverF = new DBAFBLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x09B8)));  //Traction control is the same as 185 AFB
            _trainBrakeLeverF = new BR143DriverBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0958)));
            _directBrakeLeverF = new BR143DirectBrakeLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x07B8)));


            _hornLeverBR = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0508)));
            _throttleLeverB = new BR143AFBLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x09C0)));
            _tractionLeverB = new DBAFBLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x09B0)));
            _trainBrakeLeverB = new BR143DriverBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0950)));
            _directBrakeLeverB = new BR143DirectBrakeLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x05B8)));
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
                _tractionLeverF.OnControlLoop(state.Throttle);
                _throttleLeverF.OnControlLoop(state.Reverser);
                _trainBrakeLeverF.OnControlLoop(state.AutoBrakeTranslated);
                _directBrakeLeverF.OnControlLoop(state.IndependentBrake);
            }
            else
            {
                _tractionLeverB.OnControlLoop(state.Throttle);
                _throttleLeverB.OnControlLoop(state.Reverser);
                _trainBrakeLeverB.OnControlLoop(state.AutoBrakeTranslated);
                _directBrakeLeverB.OnControlLoop(state.IndependentBrake);
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

        public string Name => "DB BR143";

        private bool _rearCab;
        private int _hornUse;
        private readonly TSWLever _hornLeverFR;
        private readonly BR143AFBLever _throttleLeverF;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private readonly DBAFBLever _tractionLeverF;
        private readonly BR143DriverBrake _trainBrakeLeverF;
        private readonly BR143DirectBrakeLever _directBrakeLeverF;
        private readonly HornLever _hornLeverBR;
        private readonly BR143AFBLever _throttleLeverB;
        private readonly DBAFBLever _tractionLeverB;
        private readonly BR143DriverBrake _trainBrakeLeverB;
        private readonly BR143DirectBrakeLever _directBrakeLeverB;
    }
}
