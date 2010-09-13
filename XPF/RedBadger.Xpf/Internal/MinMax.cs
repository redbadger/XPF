namespace RedBadger.Xpf.Internal
{
    using System;

    using RedBadger.Xpf.Presentation;

    internal struct MinMax
    {
        internal readonly double MaxHeight;

        internal readonly double MaxWidth;

        internal readonly double MinHeight;

        internal readonly double MinWidth;

        internal MinMax(UIElement element)
        {
            this.MaxHeight = element.MaxHeight;
            this.MinHeight = element.MinHeight;
            double height = element.Height;

            double explicitHeight = double.IsNaN(height) ? double.PositiveInfinity : height;
            this.MaxHeight = Math.Max(Math.Min(explicitHeight, this.MaxHeight), this.MinHeight);
            explicitHeight = double.IsNaN(height) ? 0 : height;
            this.MinHeight = Math.Max(Math.Min(this.MaxHeight, explicitHeight), this.MinHeight);

            this.MaxWidth = element.MaxWidth;
            this.MinWidth = element.MinWidth;
            double width = element.Width;

            double explicitWidth = double.IsNaN(width) ? double.PositiveInfinity : width;
            this.MaxWidth = Math.Max(Math.Min(explicitWidth, this.MaxWidth), this.MinWidth);
            explicitWidth = double.IsNaN(width) ? 0 : width;
            this.MinWidth = Math.Max(Math.Min(this.MaxWidth, explicitWidth), this.MinWidth);
        }
    }
}