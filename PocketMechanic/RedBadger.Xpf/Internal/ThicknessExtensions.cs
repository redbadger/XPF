namespace RedBadger.Xpf.Internal
{
    using System.Windows;

    internal static class ThicknessExtensions
    {
        public static Size Collapse(this Thickness thickness)
        {
            return new Size(thickness.Left + thickness.Right, thickness.Top + thickness.Bottom);
        }

        public static bool IsDifferentFrom(this Thickness thickness1, Thickness thickness2)
        {
            return thickness1.Left.IsDifferentFrom(thickness2.Left) ||
                   thickness1.Right.IsDifferentFrom(thickness2.Right) || thickness1.Top.IsDifferentFrom(thickness2.Top) ||
                   thickness1.Bottom.IsDifferentFrom(thickness2.Bottom);
        }
    }
}