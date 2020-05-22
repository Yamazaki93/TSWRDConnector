using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.DB
{
    // 1 = full throttle
    // -1 = full dynamic brake
    class BR422_MasterController : TSWLever
    {
        public BR422_MasterController(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr)
        {
        }

        public float TranslateCombinedValue(float throttle, float dynamicBrake)
        {
            if (throttle > 0.02)  // throttle is 0.55 - 0.1
            {
                return Math.Min(0.45f * throttle + 0.55f, 1f);
            }

            if (dynamicBrake > 0.02) // brake is 0.45 - 0.1
            {
                return Math.Max(0.45f - 0.35f * dynamicBrake, 0.1f);
            }

            return 0.5f;
        }
        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.ThrottleIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.ThrottleDecrease;
    }
}
