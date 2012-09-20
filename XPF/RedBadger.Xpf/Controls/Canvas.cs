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

    public class Canvas : Panel
    {
        public static readonly ReactiveProperty<double> LeftProperty = ReactiveProperty<double>.Register(
            "Left", typeof(Canvas), double.NaN, ReactivePropertyChangedCallbacks.InvalidateArrange);

        public static readonly ReactiveProperty<double> TopProperty = ReactiveProperty<double>.Register(
            "Top", typeof(Canvas), double.NaN, ReactivePropertyChangedCallbacks.InvalidateArrange);

        public static double GetLeft(IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return element.GetValue(LeftProperty);
        }

        public static double GetTop(IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return element.GetValue(TopProperty);
        }

        public static void SetLeft(IElement element, double value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(LeftProperty, value);
        }

        public static void SetTop(IElement element, double value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(TopProperty, value);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (IElement element in this.GetVisualChildren())
            {
                if (element == null)
                {
                    continue;
                }

                double x = 0.0;
                double y = 0.0;

                double left = GetLeft(element);
                if (!double.IsNaN(left))
                {
                    x = left;
                }

                double top = GetTop(element);
                if (!double.IsNaN(top))
                {
                    y = top;
                }

                element.Arrange(new Rect(new Point(x, y), element.DesiredSize));
            }

            return finalSize;
        }

        protected override Rect GetClippingRect(Size finalSize)
        {
            return Rect.Empty;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var infiniteAvailableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

            foreach (IElement element in this.GetVisualChildren())
            {
                if (element != null)
                {
                    element.Measure(infiniteAvailableSize);
                }
            }

            return new Size();
        }
    }
}
