using System;
using Memory;

namespace TSWMod.TSW.DB
{
    class BR143DirectBrakeLever : TSWLever
    {
        public BR143DirectBrakeLever(Mem mem, IntPtr hWnd, UIntPtr basePtr) : base(mem, hWnd, basePtr)
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

        protected override InputHelpers.VKCodes[] IncreaseKeys =>
            KeyboardLayoutManager.Current.IndependentBrakeIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys =>
            KeyboardLayoutManager.Current.IndependentBrakeDecrease;
    }
}
