namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    public class Canvas : Panel
    {
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
    }
}