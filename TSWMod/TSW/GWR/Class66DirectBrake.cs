using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.GWR
{
    class Class66DirectBrake : TSWLever
    {
        public Class66DirectBrake(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, true)
        {
            NotchRampUp = 3;
        }
        protected override float TargetValuePreTransform(float raw)
        {
            if (raw < 0.35)
            {
                raw = -1;
            }
            else if (raw < 0.65)
            {
                raw = 0;
            }
            else
            {
                raw = 1;
            }

            return raw;
        }
        protected override InputHelpers.VKCodes[] IncreaseKeys =>
            KeyboardLayoutManager.Current.IndependentBrakeIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys =>
            KeyboardLayoutManager.Current.IndependentBrakeDecrease;
    }
}
