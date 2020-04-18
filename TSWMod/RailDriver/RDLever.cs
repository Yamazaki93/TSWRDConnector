using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.RailDriver
{
    class RDLever
    {
        public RDLever(int min, int max)
        {
            Min = min;
            Max = max;
        }
        public int Min { get; }  // Further from user (or left for rotary) raw value. Corresponds to calibration file "Min"
        public int Max { get; }  // Close to user (or right for rotary) raw value. Corresponds to calibration file "Max"
        public int CurrentValue { get; set; }
        public float CurrentValueScaled => Math.Min(Math.Max(1 - ((float)CurrentValue - Min) / ((float)Max - Min), 0), 1f);   // Normalized value with closer to user (right) = 0, further from user (left) = 1
    }
}
