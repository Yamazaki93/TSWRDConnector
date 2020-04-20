using System;
using Memory;

namespace TSWMod.TSW
{
    class HornLever : TSWLever
    {
        public HornLever(Mem m, UIntPtr basePtr) : base(m, IntPtr.Zero, basePtr)
        {
        }

        protected override void OnDifferentValue(float diff)
        {
        }

        protected override void OnSameValue()
        {
        }
    }
}
