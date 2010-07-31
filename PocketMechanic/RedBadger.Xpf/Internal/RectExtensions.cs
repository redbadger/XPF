namespace RedBadger.Xpf.Internal
{
    using RedBadger.Xpf.Presentation;

    public static class RectExtensions
    {
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