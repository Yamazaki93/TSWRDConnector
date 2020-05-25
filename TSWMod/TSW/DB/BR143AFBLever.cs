using System;
using Memory;

namespace TSWMod.TSW.DB
{
    class BR143AFBLever : TSWLever
    {
        public BR143AFBLever(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw < 0.1)
            {
                return 0;
            }

            return raw;
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.ThrottleIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.ThrottleDecrease;
    }
}
