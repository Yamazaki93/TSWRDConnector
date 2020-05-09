using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.GWR
{
    class Class66AutoBrake : TSWLever
    {
        public Class66AutoBrake(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, false)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {

            // the values overrun to force the lever be pressed
            if (raw < 0.35)
            {
                return -1.5f;
            }
            else if (raw > 0.65)
            {
                return 1.5f;
            }

            return 0;
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.AutomaticBrakeIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.AutomaticBrakeDecrease;
    }
}
