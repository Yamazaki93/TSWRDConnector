using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.RailDriver
{
    class RDAutoBrake : RDLever
    {

        public RDAutoBrake(int min, int max, int csMin, int csMax) : base(min, max)
        {
            _csMin = csMin;
            _csMax = csMax;
        }

        public float TranslatedValue  // 0 - 1 for brake range, 2 for emergency brake
        {
            get
            {
                var csEmg = (_csMin + Min) / 2f;
                if (CurrentValue > csEmg)
                {
                    return 1 - ((float)CurrentValue - _csMin) / ((float)Max - _csMin);
                }
                return 2;
            }
        }
        private readonly int _csMin;
        private readonly int _csMax;
    }
}
