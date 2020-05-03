using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.CSX
{
    class ContinuousWiperLever : GenericWiperSelector
    {
        public ContinuousWiperLever(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, false)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw.AlmostEquals(1))
            {
                return 0.33f;
            }

            if (raw.AlmostEquals(2))
            {
                return 1;
            }

            return 0;
        }
    }
}
