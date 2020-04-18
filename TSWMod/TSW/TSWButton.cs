using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW
{
    class TSWButton
    {
        protected const ulong AbsoluteValueOffset = 0x0550;   // Absolute value offset

        protected TSWButton(Mem m, IntPtr hWnd, UIntPtr basePtr)
        {
            _m = m;
            _hWnd = hWnd;
            _basePtr = basePtr;
        }

        public bool CurrentValue => _m.ReadInt(_m.GetCodeRepresentation((UIntPtr)((ulong)_basePtr + AbsoluteValueOffset))) == 1;
        private readonly Mem _m;
        private readonly IntPtr _hWnd;
        private readonly UIntPtr _basePtr;
    }
}
