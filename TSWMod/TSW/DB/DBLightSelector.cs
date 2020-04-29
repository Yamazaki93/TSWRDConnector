using System;
using Memory;

namespace TSWMod.TSW.DB
{
    class DBLightSelector : TSWLever
    {
        public DBLightSelector(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, true)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            return raw + 1;   //mapping from 0, 1, 2 => 1, 2, 3
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.LightIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.LightDecrease;
    }
}
