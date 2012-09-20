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

    public class StackPanel : Panel
    {
        public static readonly ReactiveProperty<Orientation> OrientationProperty =
            ReactiveProperty<Orientation>.Register(
                "Orientation", 
                typeof(StackPanel), 
                Orientation.Vertical, 
                ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public Orientation Orientation
        {
            get
            {
                return this.GetValue(OrientationProperty);
            }

            set
            {
                this.SetValue(OrientationProperty, value);
            }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            bool isHorizontalOrientation = this.Orientation == Orientation.Horizontal;
            var finalRect = new Rect(new Point(), arrangeSize);
            double width = 0;
            double height = 0;
            foreach (UIElement child in this.GetVisualChildren())
            {
                if (child != null)
                {
                    if (isHorizontalOrientation)
                    {
                        finalRect.X += width;
                        width = child.DesiredSize.Width;
                        finalRect.Width = width;
                        finalRect.Height = Math.Max(arrangeSize.Height, child.DesiredSize.Height);
                    }
                    else
                    {
                        finalRect.Y += height;
                        height = child.DesiredSize.Height;
                        finalRect.Height = height;
                        finalRect.Width = Math.Max(arrangeSize.Width, child.DesiredSize.Width);
                    }

                    child.Arrange(finalRect);
                }
            }

            return arrangeSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = new Size();
            bool isHorizontalOrientation = this.Orientation == Orientation.Horizontal;
            if (isHorizontalOrientation)
            {
                availableSize.Width = double.PositiveInfinity;
            }
            else
            {
                availableSize.Height = double.PositiveInfinity;
            }

            foreach (UIElement child in this.GetVisualChildren())
            {
                if (child == null)
                {
                    continue;
                }

                child.Measure(availableSize);
                Size desiredSize = child.DesiredSize;
                if (isHorizontalOrientation)
                {
                    size.Width += desiredSize.Width;
                    size.Height = Math.Max(size.Height, desiredSize.Height);
                }
                else
                {
                    size.Width = Math.Max(size.Width, desiredSize.Width);
                    size.Height += desiredSize.Height;
                }
            }

            return size;
        }
    }
}
