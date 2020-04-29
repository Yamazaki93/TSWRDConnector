using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.Caltrains
{
    class F40PHDynamicBrake : TSWLever
    {
        public F40PHDynamicBrake(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw < 0.01)
            {
                return 0;
            }

            if (raw.AlmostEquals(0.02f))
            {
                return 0.15f;  //initial setup
            }

            return raw * 0.78f + 0.22f;
        }
        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.DynamicBrakeIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.DynamicBrakeDecrease;
    }
}
