namespace RedBadger.Xpf.Internal
{
    using System;
    using System.Windows;

    public static class RectExtensions
    {
        public static Rect Deflate(this Rect rect, Thickness thickness)
        {
            return new Rect(
                rect.X + thickness.Left,
                rect.Y + thickness.Top,
                Math.Max(0, rect.Width - (thickness.Left + thickness.Right)),
                Math.Max(0, rect.Height - (thickness.Top + thickness.Bottom)));
        }

        public static bool IsDifferentFrom(this Rect rect1, Rect rect2)
        {
            if (rect1.IsEmpty)
            {
                return !rect2.IsEmpty;
            }

            return rect2.IsEmpty || rect1.X.IsDifferentFrom(rect2.X) || rect1.Y.IsDifferentFrom(rect2.Y) ||
                   rect1.Width.IsDifferentFrom(rect2.Width) || rect1.Height.IsDifferentFrom(rect2.Height);
        }
    }
}