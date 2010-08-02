namespace RedBadger.Xpf.Internal
{
    using RedBadger.Xpf.Presentation;

    public static class ThicknessExtensions
    {
        public static bool IsDifferentFrom(this Thickness thickness1, Thickness thickness2)
        {
            if (thickness1.IsEmpty)
            {
                return !thickness2.IsEmpty;
            }

            return thickness2.IsEmpty || thickness1.Left.IsDifferentFrom(thickness2.Left) ||
                   thickness1.Right.IsDifferentFrom(thickness2.Right) || thickness1.Top.IsDifferentFrom(thickness2.Top) ||
                   thickness1.Bottom.IsDifferentFrom(thickness2.Bottom);
        }
    }
}