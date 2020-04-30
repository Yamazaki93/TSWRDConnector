using System;
using Memory;

namespace TSWMod.TSW.DB
{
    class DBDirectBrakeLever : TSWLever
    {
        public DBDirectBrakeLever(Mem mem, IntPtr hWnd, UIntPtr basePtr) : base(mem, hWnd, basePtr)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw < 0.35)
            {
                raw = 0;
            }
            else if (raw < 0.65)
            {
                raw = 0.5f;
            }
            else
            {
                raw = 1;
            }

            return 1 - raw;
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys =>
            KeyboardLayoutManager.Current.IndependentBrakeIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys =>
            KeyboardLayoutManager.Current.IndependentBrakeDecrease;
    }
}
