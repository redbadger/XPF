namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using RedBadger.Xpf.Internal.Controls;
    using RedBadger.Xpf.Presentation.Controls.Primitives;

    using Vector = RedBadger.Xpf.Presentation.Vector;

    public class VirtualizingStackPanel : StackPanel, IScrollInfo
    {
        private ScrollData scrollData;

        public bool CanHorizontallyScroll
        {
            get
            {
                return this.scrollData.CanHorizontallyScroll;
            }

            set
            {
                this.scrollData.CanHorizontallyScroll = value;
            }
        }

        public bool CanVerticallyScroll
        {
            get
            {
                return this.scrollData.CanVerticallyScroll;
            }

            set
            {
                this.scrollData.CanVerticallyScroll = value;
            }
        }

        public Size Extent
        {
            get
            {
                return this.scrollData.Extent;
            }
        }

        public Vector Offset
        {
            get
            {
                return this.scrollData.Offset;
            }
        }

        public Size Viewport
        {
            get
            {
                return this.scrollData.Viewport;
            }
        }

        public void SetHorizontalOffset(double offset)
        {
        }

        public void SetVerticalOffset(double offset)
        {
        }
    }
}