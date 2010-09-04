namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Windows;

    using RedBadger.Xpf.Internal;

    public class Canvas : Panel
    {
        public static readonly XpfDependencyProperty LeftProperty = XpfDependencyProperty.RegisterAttached(
            "Left", typeof(double), typeof(Canvas), new PropertyMetadata(double.NaN, PositionChanged));

        public static readonly XpfDependencyProperty TopProperty = XpfDependencyProperty.RegisterAttached(
            "Top", typeof(double), typeof(Canvas), new PropertyMetadata(double.NaN, PositionChanged));

        public static double GetLeft(IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (double)element.GetValue(LeftProperty.Value);
        }

        public static double GetTop(IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (double)element.GetValue(TopProperty.Value);
        }

        public static void SetLeft(IElement element, double value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(LeftProperty.Value, value);
        }

        public static void SetTop(IElement element, double value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(TopProperty.Value, value);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (var element in this.GetVisualChildren())
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

            foreach (var element in this.GetVisualChildren())
            {
                if (element != null)
                {
                    element.Measure(infiniteAvailableSize);
                }
            }

            return new Size();
        }

        private static void PositionChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var newValue = (double)args.NewValue;
            var oldValue = (double)args.OldValue;

            if (newValue.IsDifferentFrom(oldValue))
            {
                var canvas = dependencyObject as IElement;
                if (canvas != null)
                {
                    canvas.InvalidateArrange();
                }
            }
        }
    }
}