using System;
using Memory;

namespace TSWMod.TSW
{
    class GenericLightSelector : TSWLever
    {
        public GenericLightSelector(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, true)
        {
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.LightIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.LightDecrease;
    }
}
