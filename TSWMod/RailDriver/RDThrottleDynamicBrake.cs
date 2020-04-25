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

                return ((float) CurrentValue - _idleMin) / ((float) Max - _idleMin);
            }
        }

        public float TranslatedDynamicBrakeValue
        {
            get
            {
                var idleDynSetupLimit = (_dynSetupMin + _idleMin) / 2f;
                if (CurrentValue > idleDynSetupLimit)
                {
                    return 0;
                }

                var dynamicRangeValue = 1 - ((float) CurrentValue - Min) / ((float) _dynSetupMax - Min);
                if (dynamicRangeValue < 0.02)
                {
                    return 0.02f;
                }
                return Math.Max(dynamicRangeValue, 0.03f);
            }
        }

        private readonly int _idleMin;
        private readonly int _idleMax;
        private readonly int _dynSetupMin;
        private readonly int _dynSetupMax;
    }
}
