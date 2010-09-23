namespace RedBadger.Xpf.Internal
{
    using RedBadger.Xpf.Presentation;

    internal struct MinMax
    {
        internal readonly double MaxHeight;

        internal readonly double MaxWidth;

        internal readonly double MinHeight;

        internal readonly double MinWidth;

        internal MinMax(UIElement element)
        {
            double height = element.Height;
            double minHeight = element.MinHeight;
            double maxHeight = element.MaxHeight;
            this.MaxHeight = (double.IsNaN(height) ? double.PositiveInfinity : height).Coerce(minHeight, maxHeight);
            this.MinHeight = (double.IsNaN(height) ? 0 : height).Coerce(minHeight, maxHeight);

            double width = element.Width;
            double minWidth = element.MinWidth;
            double maxWidth = element.MaxWidth;
            this.MaxWidth = (double.IsNaN(width) ? double.PositiveInfinity : width).Coerce(minWidth, maxWidth);
            this.MinWidth = (double.IsNaN(width) ? 0 : width).Coerce(minWidth, maxWidth);
        }
    }
}