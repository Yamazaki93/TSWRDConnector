using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.CSX
{
    class AC4400CWAutoBrake : TSWLever
    {
        public AC4400CWAutoBrake(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, false)
        {
        }

        protected override float TargetValuePreTransform(float raw)  // raw is translated RD
        {
            if (raw.AlmostEquals(2))
            {
                return 1; // emergency brake
            }

            // all return scaled by 0.85
            if (raw < 0.117)
            {
                return 0;  // release
            }

            if (raw < 0.294)
            {
                return 0.2f;  // 0.1 - 0.25 = min reduction range
            }

            if (raw < 0.882)
            {
                return 0.2f + 0.4f * raw; // 0.2 - 0.6 = brake range;
            }

            if (raw < 1)
            {
                return 0.65f;
            }
            return 0.85f;   // > 0.85 = handle off
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.AutomaticBrakeIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.AutomaticBrakeDecrease;
    }
}
