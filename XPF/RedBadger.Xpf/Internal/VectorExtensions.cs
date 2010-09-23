namespace RedBadger.Xpf.Internal
{
    internal static class VectorExtensions
    {
        public static bool IsCloseTo(this Vector value1, Vector value2)
        {
            return !value1.IsDifferentFrom(value2);
        }

        public static bool IsDifferentFrom(this Vector value1, Vector value2)
        {
            return value1.X.IsDifferentFrom(value2.X) || value1.Y.IsDifferentFrom(value2.Y);
        }
    }
}