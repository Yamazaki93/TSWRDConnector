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
        }

        public bool CheckPlayerCalibration()
        {
            if (!_hornButtonF.CurrentValue.AlmostEquals(0.5f))
            {
                _hornUse += 1;
                _rearCab = false;
            }

            if (!_hornButtonB.CurrentValue.AlmostEquals(0.5f))
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
            _throttle.OnControlLoop(state.ThrottleTranslated);
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
        private readonly EightNotchThrottle _throttle;
        private bool _rearCab;
    }
}
