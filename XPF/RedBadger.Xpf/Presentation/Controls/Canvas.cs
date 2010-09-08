namespace RedBadger.Xpf.Presentation.Controls
{
    using System;

    using RedBadger.Xpf.Internal;

    public class Canvas : Panel
    {
        public static readonly ReactiveProperty<double, Canvas> LeftProperty = ReactiveProperty<double, Canvas>.Register(
            "Left", double.NaN, ReactivePropertyChangedCallbacks.InvalidateArrange);

        public static readonly ReactiveProperty<double, Canvas> TopProperty = ReactiveProperty<double, Canvas>.Register(
            "Top", double.NaN, ReactivePropertyChangedCallbacks.InvalidateArrange);

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