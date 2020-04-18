using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.RailDriver
{
    class RDReverser : RDLever
    {
        public RDReverser(int min, int max, int neutralMin, int neutralMax) : base(min, max)
        {
            _neutralMin = neutralMin;
            _neutralMax = neutralMax;
        }

        public float TranslatedValue
        {
            get
            {
                var forward = (_neutralMin + Min) / 2f;
                var backward = (_neutralMax + Max) / 2f;
                if (CurrentValue < forward)  // forward
                {
                    return 1;
                }
                if (CurrentValue > backward)  // reverse
                {
                    return 0;
                }
                else  // neutral
                {
                    return 0.5f;
                }
            }
        }
        private readonly int _neutralMin;
        private readonly int _neutralMax;
    }
}
