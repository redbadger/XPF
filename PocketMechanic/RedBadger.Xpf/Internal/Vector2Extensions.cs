namespace RedBadger.Xpf.Internal
{
    using Microsoft.Xna.Framework;

    public static class Vector2Extensions
    {
        public static bool IsDifferentFrom(this Vector2 value1, Vector2 value2)
        {
            return value1.X.IsDifferentFrom(value2.X) && value1.Y.IsDifferentFrom(value2.Y);
        }
    }
}