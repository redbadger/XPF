namespace RedBadger.Xpf.Internal
{
    using System;

    internal static class DoubleExtensions
    {
        public static double Coerce(this double offset, double min, double max)
        {
            return Math.Max(Math.Min(offset, max), min);
        }

        public static double EnsurePositive(this double value)
        {
            return Math.Max(value, 0.0);
        }

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

            double epsilon = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * 1e-15;
            double difference = value1 - value2;
            return !(-epsilon < difference && difference < epsilon);
        }

        public static bool IsGreaterThan(this double value1, double value2)
        {
            return value1 > value2 && value1.IsDifferentFrom(value2);
        }

        public static bool IsGreaterThanOrCloseTo(this double value1, double value2)
        {
            return value1 > value2 || value1.IsCloseTo(value2);
        }

        public static bool IsLessThan(this double value1, double value2)
        {
            return value1 < value2 && value1.IsDifferentFrom(value2);
        }

        public static bool IsLessThanOrCloseTo(this double value1, double value2)
        {
            return value1 < value2 || value1.IsCloseTo(value2);
        }
    }
}