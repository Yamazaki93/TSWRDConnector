using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.GWR
{
    class HSTBrake : TSWLever
    {
        public HSTBrake(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, true)
        {
            NotchRampUp = 1;
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw > 1)
            {
                return 6f;
            }
            return (float)Math.Round(raw * 6, 0);
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.AutomaticBrakeIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.AutomaticBrakeDecrease;
    }
}
