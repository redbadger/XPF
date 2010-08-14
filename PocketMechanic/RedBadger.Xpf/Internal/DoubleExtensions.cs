namespace RedBadger.Xpf.Internal
{
    using System;

    public static class DoubleExtensions
    {
        public static bool IsCloseTo(this double value1, double value2)
        {
            return !value1.IsDifferentFrom(value2);
        }

        public static bool IsDifferentFrom(this double value1, double double2)
        {
            if (value1 == double2)
            {
                return false;
            }

            double epsilon = (Math.Abs(value1) + Math.Abs(double2) + 10.0) * 2.2204460492503131E-8;
            double difference = value1 - double2;
            return !(-epsilon < difference && difference < epsilon);
        }

        public static bool IsGreaterThanOrCloseTo(this double value1, double value2)
        {
            if (value1 <= value2)
            {
                return value1.IsCloseTo(value2);
            }

            return true;
        }

        public static bool IsLessThan(this double value1, double value2)
        {
            return value1 < value2 && value1.IsDifferentFrom(value2);
        }

        public static bool IsGreaterThan(this double value1, double value2)
        {
            return value1 > value2 && value1.IsDifferentFrom(value2);
        }

        public static bool IsLessThanOrCloseTo(this double value1, double value2)
        {
            if (value1 >= value2)
            {
                return value1.IsCloseTo(value2);
            }

            return true;
        }
    }
}