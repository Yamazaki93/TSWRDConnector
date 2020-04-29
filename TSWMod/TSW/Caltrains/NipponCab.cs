using System;
using System.Collections.Generic;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;
using TSWMod.TSW.CSX;

namespace TSWMod.TSW.Caltrains
{
    class NipponCab : ILocomotive
    {
        public const string NamePartial = "CT_Nippon";
        private IDictionary<int, InputHelpers.VKCodes[]> DefaultKeyMappings =>
            new Dictionary<int, InputHelpers.VKCodes[]>
            {
                {36, KeyboardLayoutManager.Current.EmergencyBrake}, // Emergency brake
                {37, KeyboardLayoutManager.Current.EmergencyBrake},
                {38, KeyboardLayoutManager.Current.AlerterReset},
                {39, KeyboardLayoutManager.Current.Sand},
                {40, KeyboardLayoutManager.Current.PantographRaise},
                {41,  KeyboardLayoutManager.Current.Bell},
                {42, KeyboardLayoutManager.Current.Horn1}, // Horn
                {43, KeyboardLayoutManager.Current.Horn1},
            };

        public NipponCab(Mem m, UIntPtr basePtr, IntPtr hWnd)
        {
            _m = m;
            _basePtr = basePtr;
            _hornButton = new HornButton(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0788)));
            _throttle = new EightNotchThrottle(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0830)));
            _reverser = new PlusMinusReverser(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0840)));
            _independentBrake = new NipponCabParkingBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x06D0)));
            _autoBrake = new F40PHAutomaticBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x06F0)));
            _dynamicBrake = new SD40_2DynamicBrake(m, hWnd,
                m.GetPtr(m.GetCodeRepresentation(basePtr + 0x0828)));
        }

        public bool CheckPlayerCalibration()
        {
            if (_hornButton.CurrentValue)
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
            _throttle.OnControlLoop(state.ThrottleTranslated);
            _reverser.OnControlLoop(state.ReverserTranslated);
            _independentBrake.OnControlLoop(state.IndependentBrake);
            _autoBrake.OnControlLoop(state.AutoBrakeTranslated);
            _dynamicBrake.OnControlLoop(state.DynamicBrakeTranslated);
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

        public string Name => "NipponCab";

        private int _hornUse;
        private readonly TSWButton _hornButton;
        private readonly Mem _m;
        private readonly UIntPtr _basePtr;
        private readonly EightNotchThrottle _throttle;
        private readonly PlusMinusReverser _reverser;
        private readonly NipponCabParkingBrake _independentBrake;
        private readonly F40PHAutomaticBrake _autoBrake;
        private readonly SD40_2DynamicBrake _dynamicBrake;
    }
}
