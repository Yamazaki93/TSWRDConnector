using System;
using System.Collections.Generic;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;

namespace TSWMod.TSW.DB
{
    class DB185_2 : ILocomotive
    {
        public const string NamePartial = "DB_BR185-2";
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

        public DB185_2(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            _m = m;
            _basePtr = basePtr;
            _hornLeverFR = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0750)));
            _throttleLeverF = new DBThrottleLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0A18)));
            _afbLeverF = new DBAFBLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0A10)));
            _trainBrakeLeverF = new DBTrainBrakeLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0A08)));
            _directBrakeLeverF = new DBDirectBrakeLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0948)));

            _hornLeverBR = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0748)));
            _throttleLeverB = new DBThrottleLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0848)));
            _afbLeverB = new DBAFBLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0840)));
            _trainBrakeLeverB = new DBTrainBrakeLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0838)));
            _directBrakeLeverB = new DBDirectBrakeLever(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0720)));
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
            }
            else
            {
                _afbLeverB.OnControlLoop(state.Reverser);
                _throttleLeverB.OnControlLoop(state.Throttle);
                _trainBrakeLeverB.OnControlLoop(state.AutoBrake);
                _directBrakeLeverB.OnControlLoop(state.IndependentBrake);
            }
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
            return null;
        }

        public string Name => "DB BR185-2";

        private bool _rearCab;
        private int _hornUse;
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
    }
}
