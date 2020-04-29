using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.Caltrains
{
    class F40PHIndependentBrake : TSWLever
    {
        public F40PHIndependentBrake(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw < 0.03)
            {
                return 0;
            }

            return raw;
        }

        public void EngageBailOffIfNeeded()
        {
            if (!_bailOff && CurrentValue.AlmostEquals(0))
            {
                _bailOff = true;
                InputHelpers.KeyComboDown(HWND, KeyboardLayoutManager.Current.IndependentBrakeDecrease);
            }
        }

        public void DisengageBailOffIfNeeded()
        {
            if (_bailOff)
            {
                InputHelpers.KeyComboUp(HWND, KeyboardLayoutManager.Current.IndependentBrakeDecrease);
                _bailOff = false;
            }
        }
        protected override InputHelpers.VKCodes[] IncreaseKeys =>
            KeyboardLayoutManager.Current.IndependentBrakeIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys =>
            KeyboardLayoutManager.Current.IndependentBrakeDecrease;

        private bool _bailOff;
    }
}
