using System;
using Memory;

namespace TSWMod.TSW
{
    class GenericWiperSelector : TSWLever
    {
        public GenericWiperSelector(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, true)
        {
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.WiperIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.WiperDecrease;
    }
}
