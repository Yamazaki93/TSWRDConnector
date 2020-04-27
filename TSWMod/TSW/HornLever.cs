using System;
using Memory;

namespace TSWMod.TSW
{
    class HornLever : TSWLever
    {
        public HornLever(Mem m, UIntPtr basePtr) : base(m, IntPtr.Zero, basePtr)
        {
        }

        protected override InputHelpers.VKCodes[] IncreaseKeys => new InputHelpers.VKCodes[] { };
        protected override InputHelpers.VKCodes[] DecreaseKeys => new InputHelpers.VKCodes[] { };
    }
}
