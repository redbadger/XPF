namespace RedBadger.Xpf.Internal
{
    using RedBadger.Xpf.Presentation;

    internal static class SizeExtensions
    {
        public static Size Deflate(this Size size, Thickness thickness)
        {
            return new Size(
                (size.Width - (thickness.Left + thickness.Right)).EnsurePositive(), 
                (size.Height - (thickness.Top + thickness.Bottom)).EnsurePositive());
        }

        public static Size Inflate(this Size size, Thickness thickness)
        {
            return new Size(
                (size.Width + (thickness.Left + thickness.Right)).EnsurePositive(), 
                (size.Height + (thickness.Top + thickness.Bottom)).EnsurePositive());
        }

        public static bool IsCloseTo(this Size size1, Size size2)
        {
            return !size1.IsDifferentFrom(size2);
        }

        public static bool IsDifferentFrom(this Size size1, Size size2)
        {
            return size1.Width.IsDifferentFrom(size2.Width) || size1.Height.IsDifferentFrom(size2.Height);
        }
    }
}