namespace RedBadger.Xpf.Internal
{
    using System;

    using RedBadger.Xpf.Presentation;

    internal struct MinMax
    {
        internal readonly float MaxHeight;

        internal readonly float MaxWidth;

        internal readonly float MinHeight;

        internal readonly float MinWidth;

        internal MinMax(UIElement element)
        {
            this.MaxHeight = element.MaxHeight;
            this.MinHeight = element.MinHeight;
            float height = element.Height;

            float explicitHeight = float.IsNaN(height) ? float.PositiveInfinity : height;
            this.MaxHeight = Math.Max(Math.Min(explicitHeight, this.MaxHeight), this.MinHeight);
            explicitHeight = float.IsNaN(height) ? 0f : height;
            this.MinHeight = Math.Max(Math.Min(this.MaxHeight, explicitHeight), this.MinHeight);
            
            this.MaxWidth = element.MaxWidth;
            this.MinWidth = element.MinWidth;
            float width = element.Width;
            
            float explicitWidth = float.IsNaN(width) ? float.PositiveInfinity : width;
            this.MaxWidth = Math.Max(Math.Min(explicitWidth, this.MaxWidth), this.MinWidth);
            explicitWidth = float.IsNaN(width) ? 0f : width;
            this.MinWidth = Math.Max(Math.Min(this.MaxWidth, explicitWidth), this.MinWidth);
        }
    }
}