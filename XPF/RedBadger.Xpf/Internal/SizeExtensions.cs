namespace RedBadger.Xpf.Internal
{
    using System.Windows;

    internal static class SizeExtensions
    {
        public static bool IsDifferentFrom(this Size size1, Size size2)
        {
            return size1.Width.IsDifferentFrom(size2.Width) || size1.Height.IsDifferentFrom(size2.Height);
        }
    }
}