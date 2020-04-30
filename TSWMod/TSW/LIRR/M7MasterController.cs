using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.LIRR
{
    class M7MasterController : TSWLever
    {
        public M7MasterController(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr)
        {
        }

        public float TranslateCombinedValue(float throttle, float dynamicBrake, float autoBrake)
        {
            if (autoBrake.AlmostEquals(2))
            {
                return 0; // emergency brake takes priority
            }

            if (throttle > 0.02)  // throttle is 0.7 - 1
            {
                return Math.Min(0.7f + Convert.ToSingle(Math.Round(throttle * 0.3f, 3)), 1);
            }
            if (throttle > 0)
            {
                return 0.7f;
            }

            if (dynamicBrake > 0.02) // brake range 0.5 - 0.2
            {
                return Math.Max(0.5f - Convert.ToSingle(Math.Round(dynamicBrake * 0.3f, 3)), 0.2f);
            }

            if (dynamicBrake > 0)
            {
                return 0.5f;
            }

            return 0.6f;  // neutral = 0.6
        }
        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.ThrottleIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.ThrottleDecrease;
    }
}
