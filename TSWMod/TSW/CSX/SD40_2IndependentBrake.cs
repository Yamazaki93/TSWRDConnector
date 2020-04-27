using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.CSX
{
    class SD40_2IndependentBrake : TSWLever
    {
        public SD40_2IndependentBrake(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw < 0.03)
            {
                return 0.25f;
            }

            return raw * 0.75f + 0.25f;
        }

        public void EngageBailOffIfNeeded()
        {
            if (!_bailOff && CurrentValue.AlmostEquals(0.25f))
            {
                _bailOff = true;
                InputHelpers.KeyDown(HWND, InputHelpers.VKCodes.VK_OEM_4);
            }
        }

        public void DisengageBailOffIfNeeded()
        {
            if (_bailOff)
            {
                InputHelpers.KeyUp(HWND, InputHelpers.VKCodes.VK_OEM_4);
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
