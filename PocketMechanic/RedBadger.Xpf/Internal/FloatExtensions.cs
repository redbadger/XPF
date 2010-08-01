namespace RedBadger.Xpf.Internal
{
    using System;

    public static class FloatExtensions
    {
        public static bool IsCloseTo(this float value1, float value2)
        {
            return !value1.IsDifferentFrom(value2);
        }

        public static bool IsDifferentFrom(this float value1, float float2)
        {
            if (value1 == float2)
            {
                return false;
            }

            float epsilon = (Math.Abs(value1) + Math.Abs(float2) + 10.0f) * 2.2204460492503131E-8f;
            float difference = value1 - float2;
            return !(-epsilon < difference && difference < epsilon);
        }

        public static bool IsGreaterThanOrCloseTo(this float value1, float value2)
        {
            if (value1 <= value2)
            {
                return value1.IsCloseTo(value2);
            }

            return true;
        }

        public static bool IsLessThan(this float value1, float value2)
        {
            return value1 < value2 && value1.IsDifferentFrom(value2);
        }

        public static bool IsLessThanOrCloseTo(this float value1, float value2)
        {
            if (value1 >= value2)
            {
                return value1.IsCloseTo(value2);
            }

            return true;
        }
    }
}