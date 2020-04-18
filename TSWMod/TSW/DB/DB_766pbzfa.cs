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
        private readonly IDictionary<int, InputHelpers.VirtualKeyStates> DefaultKeyMappings = new Dictionary<int, InputHelpers.VirtualKeyStates>
        {
            { 2, InputHelpers.VirtualKeyStates.VK_NEXT },  // PZB Ack
            { 3, InputHelpers.VirtualKeyStates.VK_END },  // PZB Release
            { 4, InputHelpers.VirtualKeyStates.VK_DELETE },  // PZB Override
            { 34, InputHelpers.VirtualKeyStates.VK_W },  // Reverser
            { 35, InputHelpers.VirtualKeyStates.VK_S },  
            { 36, InputHelpers.VirtualKeyStates.VK_BACK },  // Emergency brake
            { 37, InputHelpers.VirtualKeyStates.VK_BACK },
            { 38, InputHelpers.VirtualKeyStates.VK_Q },
            { 39, InputHelpers.VirtualKeyStates.VK_X },
            { 40, InputHelpers.VirtualKeyStates.VK_P },
            { 42, InputHelpers.VirtualKeyStates.VK_SPACE },   // Horn
            { 43, InputHelpers.VirtualKeyStates.VK_N },
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

        public string Name => "DB 766pbzfa";

        private int _hornUse;
        private readonly TSWLever _hornLeverFR;
        private readonly TSWLever _throttleLeverF;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private readonly DBTrainBrakeLever _trainBrakeLeverF;
        private readonly DBDirectBrakeLever _directBrakeLeverF;
    }
}
