namespace RedBadger.Xpf.Internal.Controls
{
    using RedBadger.Xpf.Presentation;

    internal struct ScrollData
    {
        public bool CanHorizontallyScroll;

        public bool CanVerticallyScroll;

        public Size Extent;

        public Vector Offset;

        public Size Viewport;
    }
}