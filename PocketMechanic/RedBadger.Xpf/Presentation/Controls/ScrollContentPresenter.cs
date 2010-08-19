namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Windows;

    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Internal.Controls;
    using RedBadger.Xpf.Presentation.Controls.Primitives;

    using Vector = RedBadger.Xpf.Presentation.Vector;

    public class ScrollContentPresenter : ContentControl, IScrollInfo
    {
        private ScrollData scrollData;

        public ScrollContentPresenter()
        {
            this.scrollData.CanHorizontallyScroll = true;
            this.scrollData.CanVerticallyScroll = true;
        }

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
            if (this.CanHorizontallyScroll)
            {
                offset = ValidateInputOffset(offset);
                if (this.scrollData.Offset.X.IsDifferentFrom(offset))
                {
                    this.scrollData.Offset.X = offset;
                    this.InvalidateArrange();
                }
            }
        }

        public void SetVerticalOffset(double offset)
        {
            if (this.CanVerticallyScroll)
            {
                offset = ValidateInputOffset(offset);
                if (this.scrollData.Offset.Y.IsDifferentFrom(offset))
                {
                    this.scrollData.Offset.Y = offset;
                    this.InvalidateArrange();
                }
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            IElement content = this.Content;

            this.UpdateScrollData(finalSize, this.scrollData.Extent);

            if (content != null)
            {
                var finalRect = new Rect(0.0, 0.0, content.DesiredSize.Width, content.DesiredSize.Height)
                    {
                       X = -this.scrollData.Offset.X, Y = -this.scrollData.Offset.Y 
                    };

                finalRect.Width = Math.Max(finalRect.Width, finalSize.Width);
                finalRect.Height = Math.Max(finalRect.Height, finalSize.Height);

                content.Arrange(finalRect);
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            IElement content = this.Content;
            var desiredSize = new Size();
            var extent = new Size();

            if (content != null)
            {
                Size availableSizeForContent = availableSize;
                if (this.scrollData.CanHorizontallyScroll)
                {
                    availableSizeForContent.Width = double.PositiveInfinity;
                }

                if (this.scrollData.CanVerticallyScroll)
                {
                    availableSizeForContent.Height = double.PositiveInfinity;
                }

                content.Measure(availableSizeForContent);
                desiredSize = content.DesiredSize;

                extent = content.DesiredSize;
            }

            this.UpdateScrollData(availableSize, extent);
            desiredSize.Width = Math.Min(availableSize.Width, desiredSize.Width);
            desiredSize.Height = Math.Min(availableSize.Height, desiredSize.Height);

            return desiredSize;
        }

        private static double CoerceOffset(double offset, double extent, double viewport)
        {
            if (offset > (extent - viewport))
            {
                offset = extent - viewport;
            }

            if (offset < 0.0)
            {
                offset = 0.0;
            }

            return offset;
        }

        private static double ValidateInputOffset(double offset)
        {
            if (double.IsNaN(offset))
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            return Math.Max(0.0, offset);
        }

        private void UpdateScrollData(Size viewport, Size extent)
        {
            this.scrollData.Viewport = viewport;
            this.scrollData.Extent = extent;

            var x = CoerceOffset(this.scrollData.Offset.X, this.scrollData.Extent.Width, this.scrollData.Viewport.Width);
            var y = CoerceOffset(
                this.scrollData.Offset.Y, this.scrollData.Extent.Height, this.scrollData.Viewport.Height);

            this.scrollData.Offset = new Vector(x, y);
        }
    }
}