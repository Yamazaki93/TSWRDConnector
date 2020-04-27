using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.CSX
{
    class SD40_2DynamicBrake : TSWLever
    {
        public SD40_2DynamicBrake(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr)
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
                return 0.14f;  //initial setup
            }

            return raw * 0.8f + 0.2f;
        }
        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.DynamicBrakeIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.DynamicBrakeDecrease;
    }
}
