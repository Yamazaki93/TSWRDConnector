using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.Caltrains
{
    class NipponCabParkingBrake : TSWLever
    {
        public NipponCabParkingBrake(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw < 0.5)
            {
                return 0;
            }

            return 1;
        }
        protected override InputHelpers.VKCodes[] IncreaseKeys =>
            KeyboardLayoutManager.Current.IndependentBrakeIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys =>
            KeyboardLayoutManager.Current.IndependentBrakeDecrease;

    }
}
