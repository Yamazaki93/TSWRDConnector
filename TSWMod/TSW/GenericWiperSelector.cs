using System;
using Memory;

namespace TSWMod.TSW
{
    class GenericWiperSelector : TSWLever
    {
        public GenericWiperSelector(Mem m, IntPtr hWnd, UIntPtr basePtr, bool isNotch = true) : base(m, hWnd, basePtr, isNotch)
        {
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.WiperIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.WiperDecrease;
    }
}
