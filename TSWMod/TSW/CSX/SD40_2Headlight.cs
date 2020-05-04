using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.CSX
{
    class SD40_2Headlight : GenericLightSelector
    {
        public SD40_2Headlight(Mem m, IntPtr hWnd, UIntPtr basePtr, int rampUp = 10, bool discrete = false) : base(m, hWnd, basePtr)
        {
            NotchRampUp = rampUp;
            DiscreteClickRequired = discrete;
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw.AlmostEquals(1f))
            {
                return 2f;
            }

            if (raw.AlmostEquals(2f))
            {
                return 3f;
            }

            return raw;
        }
    }
}
