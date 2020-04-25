using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;
using TSWMod.TSW.CSX;

namespace TSWMod.TSW.LIRR
{
    class M7 : ILocomotive
    {
        public const string NamePartial = "LIRR_M7";
        private readonly IDictionary<int, InputHelpers.VirtualKeyStates[]> DefaultKeyMappings = new Dictionary<int, InputHelpers.VirtualKeyStates[]>
        {
            { 34, new []{InputHelpers.VirtualKeyStates.VK_W }},  // Reverser
            { 35, new []{InputHelpers.VirtualKeyStates.VK_S }},
            { 36, new []{InputHelpers.VirtualKeyStates.VK_BACK }},  // Emergency brake
            { 37, new []{InputHelpers.VirtualKeyStates.VK_BACK }},
            { 38, new []{InputHelpers.VirtualKeyStates.VK_Q }},
            { 39, new []{InputHelpers.VirtualKeyStates.VK_X }},
            { 40, new []{InputHelpers.VirtualKeyStates.VK_P }},
            { 41, new []{InputHelpers.VirtualKeyStates.VK_B }},
            { 42, new []{InputHelpers.VirtualKeyStates.VK_SPACE }},   // Horn
            { 43, new []{InputHelpers.VirtualKeyStates.VK_SPACE }},
        };

        public M7(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            _m = m;
            _basePtr = basePtr;
            _hornButton = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x05A8)));
            _masterController = new M7MasterController(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0668)));
        }

        public bool CheckPlayerCalibration()
        {
            if (_hornButton.CurrentValue > 0.8)
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

        public IDictionary<int, InputHelpers.VirtualKeyStates[]> GetButtonMappings()
        {
            return DefaultKeyMappings;
        }

        public JObject GetConfiguration()
        {
            return null;
        }

        public string Name => "M7-A";

        private int _hornUse;
        private readonly TSWLever _hornButton;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private readonly M7MasterController _masterController;
    }
}
