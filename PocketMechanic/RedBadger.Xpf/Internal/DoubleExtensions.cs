namespace RedBadger.Xpf.Internal
{
    using System;

    public static class DoubleExtensions
    {
        public static bool IsCloseTo(this double value1, double value2)
        {
            return !value1.IsDifferentFrom(value2);
        }

        public static bool IsDifferentFrom(this double value1, double value2)
        {
            if (value1 == value2)
            {
                return false;
            }

            double epsilon = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * 1.1102230246251568E-16;
            double difference = value1 - value2;
            return !(-epsilon < difference && difference < epsilon);
        }

        public static bool IsGreaterThan(this double value1, double value2)
        {
            return value1 > value2 && value1.IsDifferentFrom(value2);
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