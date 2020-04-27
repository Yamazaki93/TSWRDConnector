using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.CSX
{
    // Reverser with notch, +1 = Forward, 0 = Neutral, -1 = Reverse
    class PlusMinusReverser : TSWLever
    {
        public PlusMinusReverser(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, true)
        {
            NotchRampUp = 7;
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw.AlmostEquals(1))
            {
                raw = 1;
            }
            else if (raw.AlmostEquals(0.5f))
            {
                raw = 0;
            }
            else
            {
                raw = -1;
            }

            return raw;
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.ReverserIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.ReverserDecrease;
    }
}
