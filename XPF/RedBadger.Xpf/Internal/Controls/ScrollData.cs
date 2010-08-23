namespace RedBadger.Xpf.Internal.Controls
{
    using System.Windows;

    using Vector = RedBadger.Xpf.Presentation.Vector;

    internal struct ScrollData
    {
        public bool CanHorizontallyScroll;

        public bool CanVerticallyScroll;

        public Size Extent;

        public Vector Offset;

        public Size Viewport;
    }
}