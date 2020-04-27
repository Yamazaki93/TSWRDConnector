using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.CSX
{
    class AC4400CWThrottle : TSWLever
    {
        public AC4400CWThrottle(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, true)
        {
        }

        public float TranslateCombinedValue(float throttle, float dynamicBrake)
        {
            if (throttle > 0)  // throttle is 0-8
            {
                return Convert.ToSingle(Math.Round(throttle * 8f, 0));
            }

            if (dynamicBrake > 0) // dynamic brake is -1 - -9
            {
                return Convert.ToSingle(Math.Round(-1 - (dynamicBrake * 8f)));
            }

            return 0;
        }
        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.ThrottleIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.ThrottleDecrease;
    }
}
