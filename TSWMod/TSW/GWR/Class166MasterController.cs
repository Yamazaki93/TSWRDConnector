using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.GWR
{
    class Class166MasterController : TSWLever
    {
        public Class166MasterController(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, true)
        {
        }

        public float TranslateCombinedValue(float throttle, float dynamicBrake, float autoBrake)
        {
            if (autoBrake.AlmostEquals(2))
            {
                return -4; // emergency brake takes priority
            }

            if (dynamicBrake > 0.02)  // brake is 0 - -3
            {
                return (float)Math.Round(dynamicBrake * -3, 0);
            }

            if (throttle > 0.02) // throttle 0 - 7
            {
                return (float)Math.Round(throttle * 7, 0);
            }


            return 0f;  // neutral = 0
        }
        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.ThrottleIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.ThrottleDecrease;
    }
}
