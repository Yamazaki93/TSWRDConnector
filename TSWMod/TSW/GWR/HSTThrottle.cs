using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.GWR
{
    class HSTThrottle : TSWLever
    {
        public HSTThrottle(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, true)
        {
            NotchRampUp = 5;
        }

        protected override float TargetValuePreTransform(float raw)
        {
            return (float)Math.Round(raw * 5, 0);
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.ThrottleIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.ThrottleDecrease;
    }
}
