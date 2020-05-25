using System;
using Memory;

namespace TSWMod.TSW.DB
{
    class BR143DriverBrake : TSWLever
    {
        public BR143DriverBrake(Mem mem, IntPtr hWnd, UIntPtr basePtr) : base(mem, hWnd, basePtr, false)
        {
        }

        protected override float TargetValuePreTransform(float raw)  // brake translated
        {
            if (raw < 0.1)
            {
                return -0.5f;   //force release press
            }

            if (raw < 0.2)
            {
                return 0.1f;
            }

            if (raw < 0.3)
            {
                return 0.2f;
            }

            if (raw.AlmostEquals(2))
            {
                return 1;
            }

            return Math.Min(0.5f * raw + 0.3f, 0.8f);
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.AutomaticBrakeIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.AutomaticBrakeDecrease;
    }
}
