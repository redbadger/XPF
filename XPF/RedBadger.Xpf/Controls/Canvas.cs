﻿namespace RedBadger.Xpf.Controls
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