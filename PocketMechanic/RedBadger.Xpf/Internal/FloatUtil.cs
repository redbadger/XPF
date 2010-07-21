namespace RedBadger.Xpf.Internal
{
    using System;

    public class FloatUtil
    {
        public static bool AreClose(float value1, float value2)
        {
            if (value1 == value2)
            {
                return true;
            }

            float epsilon = (Math.Abs(value1) + Math.Abs(value2) + 10.0f) * 2.2204460492503131E-8f;
            float difference = value1 - value2;
            return -epsilon < difference && epsilon > difference;
        }

        public static bool LessThan(float value1, float value2)
        {
            return value1 < value2 && !AreClose(value1, value2);
        }
    }
}