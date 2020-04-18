using System;

namespace TSWMod
{
    static class FloatExtensions
    {
        public static bool AlmostEquals(this float f, float other)
        {
            return Math.Abs(f - other) < 0.01;
        }
    }
}
