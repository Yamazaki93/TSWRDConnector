using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;
using TSWMod.TSW.CSX;

namespace TSWMod.TSW.GWR
{
    class Class66 : ILocomotive
    {
        public const string NamePartial = "DBSClass66";
        private IDictionary<int, InputHelpers.VKCodes[]> DefaultKeyMappings =>
            new Dictionary<int, InputHelpers.VKCodes[]>
            {
                {34, KeyboardLayoutManager.Current.ReverserIncrease}, // Reverser
                {35, KeyboardLayoutManager.Current.ReverserDecrease},
                {36, KeyboardLayoutManager.Current.EmergencyBrake}, // Emergency brake
                {37, KeyboardLayoutManager.Current.EmergencyBrake},
                {38, KeyboardLayoutManager.Current.AlerterReset},
                {39, KeyboardLayoutManager.Current.Sand},
                {40, KeyboardLayoutManager.Current.PantographRaise},
                {41,  KeyboardLayoutManager.Current.Bell},
                {42, KeyboardLayoutManager.Current.Horn1}, // Horn
                {43, KeyboardLayoutManager.Current.Horn2},
            };

        public Class66(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            _m = m;
            _basePtr = basePtr;
            _hornButtonF = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0648)));
            _hornButtonB = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0640)));
            _reverserF = new PlusMinusReverser(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0AF8)));
            _reverserB = new PlusMinusReverser(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0890)));
            _throttleF = new EightNotchThrottle(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0B00)));
            _throttleB = new EightNotchThrottle(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0898)));
            _directBrakeF = new Class66DirectBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0A78)));
            _directBrakeB = new Class66DirectBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0810)));
            _autoBrakeF = new Class66AutoBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0A70)));
            _autoBrakeB = new Class66AutoBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0808)));
        }

        public bool CheckPlayerCalibration()
        {
            if (!_hornButtonF.CurrentValue.AlmostEquals(0))
            {
                _hornUse += 1;
                _rearCab = false;
            }
            if (!_hornButtonB.CurrentValue.AlmostEquals(0))
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
            if (_rearCab)
            {
                _reverserB.OnControlLoop(state.ReverserTranslated);
                _throttleB.OnControlLoop(state.ThrottleTranslated);
                _directBrakeB.OnControlLoop(state.IndependentBrake);
                _autoBrakeB.OnControlLoop(state.AutoBrakeTranslated);
            }
            else
            {
                _reverserF.OnControlLoop(state.ReverserTranslated);
                _throttleF.OnControlLoop(state.ThrottleTranslated);
                _directBrakeF.OnControlLoop(state.IndependentBrake);
                _autoBrakeF.OnControlLoop(state.AutoBrakeTranslated);
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

        public string Name => "Class 66";

        private int _hornUse;
        private readonly TSWLever _hornButtonF;
        private readonly TSWLever _hornButtonB;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private readonly EightNotchThrottle _throttleF;
        private bool _rearCab;
        private readonly PlusMinusReverser _reverserF;
        private readonly PlusMinusReverser _reverserB;
        private readonly EightNotchThrottle _throttleB;
        private readonly Class66DirectBrake _directBrakeF;
        private readonly Class66DirectBrake _directBrakeB;
        private readonly Class66AutoBrake _autoBrakeF;
        private readonly Class66AutoBrake _autoBrakeB;
    }
}
