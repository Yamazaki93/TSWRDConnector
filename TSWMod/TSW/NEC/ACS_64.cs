﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;
using TSWMod.TSW.CSX;

namespace TSWMod.TSW.NEC
{
    class ACS_64 : ILocomotive
    {
        public const string NamePartial = "AMTK_ACS64";
        public const string MasterControllerConfigKey = "MasterControlConfiguration";
        public const string MasterControllerFollowTSW = "TSW";
        public const string MasterControllerFollowRD = "RD";

        private readonly IDictionary<int, InputHelpers.VirtualKeyStates> DefaultKeyMappings = new Dictionary<int, InputHelpers.VirtualKeyStates>
        {
            { 34, InputHelpers.VirtualKeyStates.VK_W },  // Reverser
            { 35, InputHelpers.VirtualKeyStates.VK_S },
            { 36, InputHelpers.VirtualKeyStates.VK_BACK },  // Emergency brake
            { 37, InputHelpers.VirtualKeyStates.VK_BACK },
            { 38, InputHelpers.VirtualKeyStates.VK_Q },
            { 39, InputHelpers.VirtualKeyStates.VK_X },
            { 40, InputHelpers.VirtualKeyStates.VK_P },
            { 41, InputHelpers.VirtualKeyStates.VK_B },
            { 42, InputHelpers.VirtualKeyStates.VK_SPACE },   // Horn
            { 43, InputHelpers.VirtualKeyStates.VK_N },
        };

        public ACS_64(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            _m = m;
            _basePtr = basePtr;
            _config = new JObject
            {
                {MasterControllerConfigKey, MasterControllerFollowTSW} // TSW: follow TSW model, RD: follow RD label
            };
            _hornLeverF = new HornLever(m,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0D70)));
            _hornButtonLowF = new HornButton(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0590)));
            _hornButtonHighF = new HornButton(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0598)));

            _masterControlF = new ACSMasterController(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0CD0)));

            _independentBrakeF = new AC4400CWIndependentBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0D40)));

            _autoBrakeF = new ACS_64AutoBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0D30)));
        }

        public bool CheckPlayerCalibration()
        {
            if (!_hornLeverF.CurrentValue.AlmostEquals(0) || _hornButtonLowF.CurrentValue || _hornButtonHighF.CurrentValue)  // ACS horn 0 is neutral
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
            if (!state.BailOff)
            {
                _independentBrakeF.DisengageBailOffIfNeeded();
            }
            if (_config[MasterControllerConfigKey].Value<string>().Equals(MasterControllerFollowRD))
            {
                _masterControlF.OnControlLoop(_masterControlF.TranslateCombinedValue(state.ThrottleTranslated,
                    state.DynamicBrakeTranslated));
            }
            else if (_config[MasterControllerConfigKey].Value<string>().Equals(MasterControllerFollowTSW))
            {
                // in TSW, ACS-64 lever forward is throttle
                _masterControlF.OnControlLoop(_masterControlF.TranslateCombinedValue(state.DynamicBrakeTranslated,
                    state.ThrottleTranslated));
            }

            _independentBrakeF.OnControlLoop(state.IndependentBrake);
            _autoBrakeF.OnControlLoop(state.AutoBrakeTranslated);

            if (state.BailOff)
            {
                _independentBrakeF.EngageBailOffIfNeeded();
            }
        }

        public void Close()
        {
        }

        public void SetConfiguration(JObject config)
        {
            _config = config;
        }

        public IDictionary<int, InputHelpers.VirtualKeyStates> GetButtonMappings()
        {
            return DefaultKeyMappings;
        }

        public JObject GetConfiguration()
        {
            return _config;
        }

        public string Name => "ACS-64";

        private int _hornUse;
        private JObject _config;
        private readonly TSWLever _hornLeverF;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private readonly AC4400CWIndependentBrake _independentBrakeF;
        private readonly HornButton _hornButtonLowF;
        private readonly ACSMasterController _masterControlF;
        private readonly HornButton _hornButtonHighF;
        private readonly ACS_64AutoBrake _autoBrakeF;
    }
}
