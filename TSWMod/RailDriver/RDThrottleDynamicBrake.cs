using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.RailDriver
{
    class RDThrottleDynamicBrake : RDLever
    {
        public RDThrottleDynamicBrake(int min, int max, int idleMin, int idleMax, int dynSetupMin, int dynSetupMax, int neutralMin, int neutralMax) : base(min, max)
        {
            _idleMin = idleMin;
            _idleMax = idleMax;
            _dynSetupMin = dynSetupMin;
            _dynSetupMax = dynSetupMax;
            _neutralMin = neutralMin;
            _neutralMax = neutralMax;
        }

        public float TranslatedThrottleValue
        {
            get
            {
                if (CurrentValue < _idleMin)
                {
                    return 0;
                }

                if (CurrentValue <= _idleMax)
                {
                    return 0.02f;
                }

                var throttleRange = ((float)CurrentValue - _idleMax) / ((float)Max - _idleMax);
                return Math.Max(throttleRange, 0.03f);
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

                if (CurrentValue >= _dynSetupMin)
                {
                    return 0.02f;
                }
                var dynamicRangeValue = 1 - ((float) CurrentValue - Min) / ((float) _dynSetupMax - Min);
                return Math.Max(dynamicRangeValue, 0.03f);
            }
        }

        private readonly int _idleMin;
        private readonly int _idleMax;
        private readonly int _dynSetupMin;
        private readonly int _dynSetupMax;
        private readonly int _neutralMin;
        private readonly int _neutralMax;
    }
}
