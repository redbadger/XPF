#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

namespace RedBadger.Xpf.Controls
{
    using System;

    using RedBadger.Xpf.Controls.Primitives;
    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Internal.Controls;

    public class ScrollContentPresenter : ContentControl, IScrollInfo
    {
        private bool isClippingRequired;

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
            if (!this.CanHorizontallyScroll)
            {
                return;
            }

            if (double.IsNaN(offset))
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            offset = Math.Max(0d, offset);

            if (this.scrollData.Offset.X.IsDifferentFrom(offset))
            {
                this.scrollData.Offset.X = offset;
                this.InvalidateArrange();
            }
        }

        public void SetVerticalOffset(double offset)
        {
            if (!this.CanVerticallyScroll)
            {
                return;
            }

            if (double.IsNaN(offset))
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            offset = Math.Max(0d, offset);

            if (this.scrollData.Offset.Y.IsDifferentFrom(offset))
            {
                this.scrollData.Offset.Y = offset;
                this.InvalidateArrange();
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            IElement content = this.Content;

            this.UpdateScrollData(finalSize, this.scrollData.Extent);

            if (content != null)
            {
                var finalRect = new Rect(
                    -this.scrollData.Offset.X, 
                    -this.scrollData.Offset.Y, 
                    content.DesiredSize.Width, 
                    content.DesiredSize.Height);

                this.isClippingRequired = finalSize.Width.IsLessThan(finalRect.Width) ||
                                          finalSize.Height.IsLessThan(finalRect.Height);

                finalRect.Width = Math.Max(finalRect.Width, finalSize.Width);
                finalRect.Height = Math.Max(finalRect.Height, finalSize.Height);

                content.Arrange(finalRect);
            }

            return finalSize;
        }

        protected override Rect GetClippingRect(Size finalSize)
        {
            return this.isClippingRequired ? new Rect(this.RenderSize) : Rect.Empty;
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
}
