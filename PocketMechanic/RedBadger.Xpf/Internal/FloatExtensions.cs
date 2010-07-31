namespace RedBadger.Xpf.Internal
{
    using System;

    public static class FloatExtensions
    {
        public static bool IsDifferentFrom(this float float1, float float2)
        {
            if (float1 == float2)
            {
                return false;
            }

            float epsilon = (Math.Abs(float1) + Math.Abs(float2) + 10.0f) * 2.2204460492503131E-8f;
            float difference = float1 - float2;
            return !(-epsilon < difference && difference < epsilon);
        }

        public static bool IsLessThan(this float float1, float float2)
        {
            return float1 < float2 && float1.IsDifferentFrom(float2);
        }
    }
}