using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.RailDriver
{
    class RDWiper : RDLever
    {

        public RDWiper(int min, int max, int middle) : base(min, max)
        {
            _middle = middle;
        }

        public int GetTranslatedValue() // 0 = off, 1 = dim, 2 = bright
        {
            var toMin = Math.Abs(CurrentValue - Min);
            var toMax = Math.Abs(CurrentValue - Max);
            var toMid = Math.Abs(CurrentValue - _middle);
            if (toMin < toMid)
            {
                return 0;
            }

            if (toMid < toMax)
            {
                return 1;
            }

            return 2;
        }

        private readonly int _middle;
    }
}
