using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.Caltrains
{
    class F40PHAutomaticBrake : TSWLever
    {
        public F40PHAutomaticBrake(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, true)
        {
            NotchRampUp = 7;
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw.AlmostEquals(2f))
            {
                return 5;
            }
            return Convert.ToSingle(Math.Round(raw * 4f, 0));
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.AutomaticBrakeIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.AutomaticBrakeDecrease;
    }
}
