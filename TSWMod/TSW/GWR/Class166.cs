using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;
using TSWMod.TSW.LIRR;

namespace TSWMod.TSW.GWR
{
    class Class166 : ILocomotive
    {
        public const string NamePartial = "GWR_Class166";
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

        public Class166(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            _m = m;
            _basePtr = basePtr;
            _hornButton = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0630)));
            _masterController = new Class166MasterController(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0918)));
        }

        public bool CheckPlayerCalibration()
        {
            if (!_hornButton.CurrentValue.AlmostEquals(0f))
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
            _masterController.OnControlLoop(_masterController.TranslateCombinedValue(state.ThrottleTranslated, state.DynamicBrakeTranslated, state.AutoBrakeTranslated));
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

        public string Name => "Class 166";

        private int _hornUse;
        private readonly TSWLever _hornButton;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private readonly Class166MasterController _masterController;
    }
}
