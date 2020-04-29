using System;
using Memory;

namespace TSWMod.TSW.DB
{
    class DBWiperSelector : TSWLever
    {
        public DBWiperSelector(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, true)
        {
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.WiperIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.WiperDecrease;
    }
}
