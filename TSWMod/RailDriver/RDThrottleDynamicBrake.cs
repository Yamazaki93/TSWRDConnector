using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.RailDriver
{
    class RDThrottleDynamicBrake : RDLever
    {
        public RDThrottleDynamicBrake(int min, int max, int idleMin, int idleMax, int dynSetupMin, int dynSetupMax) : base(min, max)
        {
            _idleMin = idleMin;
            _idleMax = idleMax;
            _dynSetupMin = dynSetupMin;
            _dynSetupMax = dynSetupMax;
        }

        public float TranslatedThrottleValue
        {
            get
            {
                if (CurrentValue < _idleMin)
                {
                    return 0;
                }

                return ((float)CurrentValue - _idleMin) / ((float)Max - _idleMin);
            }
        }

        public float TranslatedDynamicBrakeValue
        {
            get
            {
                if (CurrentValue > _dynSetupMax)
                {
                    return 0;
                }

                return 1 - ((float)CurrentValue - Min) / ((float)_dynSetupMax - Min);
            }
        }

        private readonly int _idleMin;
        private readonly int _idleMax;
        private readonly int _dynSetupMin;
        private readonly int _dynSetupMax;
    }
}
