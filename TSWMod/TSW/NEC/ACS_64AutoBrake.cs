using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.NEC
{
    class ACS_64AutoBrake : TSWLever
    {
        public ACS_64AutoBrake(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr)
        {
        }

        protected override float TargetValuePreTransform(float raw)  // raw is translated RD
        {
            if (raw.AlmostEquals(2))
            {
                return 1; // emergency brake
            }

            if (raw < 0.1)
            {
                return 0;  // release
            }

            if (raw < 0.15)
            {
                return 0.15f;  // 0.1 - 0.25 = min reduction range
            }

            if (raw < 0.9)
            {
                return 0.15f + 0.6f * raw; // 0.2 - 0.75 = brake range;
            }

            if (raw < 1)
            {
                return 0.8f;  // 0.8 full service
            }
            return 0.9f;   // 0.9 = handle off
        }
        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.AutomaticBrakeIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.AutomaticBrakeDecrease;
    }
}
