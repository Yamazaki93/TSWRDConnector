using System;
using Memory;

namespace TSWMod.TSW.DB
{
    class DBLightSelector : GenericLightSelector
    {
        public DBLightSelector(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            return raw + 1;   //mapping from 0, 1, 2 => 1, 2, 3
        }
    }
}
