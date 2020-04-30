using System;
using Memory;

namespace TSWMod.TSW.DB
{
    class DBAFBLever : TSWLever
    {
        public DBAFBLever(Mem mem, IntPtr hWnd, UIntPtr basePtr) : base(mem, hWnd, basePtr)
        {
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.CruiseControlIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.CruiseControlDecrease;
    }
}