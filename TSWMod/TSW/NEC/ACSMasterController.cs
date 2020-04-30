using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.NEC
{
    // 1 = full throttle
    // -1 = full dynamic brake
    class ACSMasterController : TSWLever
    {
        public ACSMasterController(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr)
        {
        }

        public float TranslateCombinedValue(float throttle, float dynamicBrake)
        {
            if (throttle > 0)  // throttle is 0-1 continuous
            {
                return throttle;
            }

            if (dynamicBrake > 0) // dynamic brake is 0 - -1 continuous
            {
                return -1 * dynamicBrake;
            }

            return 0;
        }
        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.ThrottleIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.ThrottleDecrease;
    }
}
