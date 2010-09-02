namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Internal.Controls;
    using RedBadger.Xpf.Presentation.Controls.Primitives;

    using Vector = RedBadger.Xpf.Presentation.Vector;

/*
    public class VirtualizingStackPanel : StackPanel, IScrollInfo
    {
        private VirtualizingElementCollection children;

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

        public override IEnumerable<IElement> GetVisualChildren()
        {
            return this.children.RealizedElements;
        }

        public void SetHorizontalOffset(double offset)
        {
        }

        public void SetVerticalOffset(double offset)
        {
        }

        protected override IList<IElement> CreateChildrenCollection()
        {
            this.children = new VirtualizingElementCollection(this);
            return this.children;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var viewportUsed = new Size();

            var availableSizeForContent = availableSize;
            if (this.Orientation == Orientation.Horizontal || this.scrollData.CanHorizontallyScroll)
            {
                availableSizeForContent.Width = double.PositiveInfinity;
            }

            if (this.Orientation == Orientation.Vertical || this.scrollData.CanVerticallyScroll)
            {
                availableSizeForContent.Height = double.PositiveInfinity;
            }

            // TODO: work out what the first visible child is from the scrolldata offset
            int firstVisibleChild = 0;
            bool isLastVisibleChild = false;
            foreach (var child in this.children.GetCursor(firstVisibleChild))
            {
                if (isLastVisibleChild)
                {
                    break;
                }

                if (child != null)
                {
                    child.Measure(availableSizeForContent);
                    var childDesiredSize = child.DesiredSize;
                    if (this.Orientation == Orientation.Horizontal)
                    {
                        isLastVisibleChild = viewportUsed.Width + childDesiredSize.Width > availableSize.Width;
                        viewportUsed.Width += childDesiredSize.Width;
                        viewportUsed.Height = Math.Max(viewportUsed.Height, childDesiredSize.Height);
                    }
                    else
                    {
                        isLastVisibleChild = viewportUsed.Height + childDesiredSize.Height > availableSize.Height;
                        viewportUsed.Width = Math.Max(viewportUsed.Width, childDesiredSize.Width);
                        viewportUsed.Height += childDesiredSize.Height;
                    }
                }
            }

            this.UpdateScrollData(availableSize, viewportUsed);
            viewportUsed.Width = Math.Min(availableSize.Width, viewportUsed.Width);
            viewportUsed.Height = Math.Min(availableSize.Height, viewportUsed.Height);

            return viewportUsed;
        }

        private void UpdateScrollData(Size viewport, Size extent)
        {
            this.scrollData.Viewport = viewport;
            this.scrollData.Extent = extent;

            double x = this.scrollData.Offset.X.Coerce(
                0d, this.scrollData.Extent.Width - this.scrollData.Viewport.Width);
            double y = this.scrollData.Offset.Y.Coerce(
                0d, this.scrollData.Extent.Height - this.scrollData.Viewport.Height);

            this.scrollData.Offset = new Vector(x, y);
        }
    }
*/
}