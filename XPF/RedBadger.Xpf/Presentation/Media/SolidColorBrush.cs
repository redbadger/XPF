namespace RedBadger.Xpf.Presentation.Media
{
    public class SolidColorBrush : Brush
    {
        public static readonly IDependencyProperty ColorProperty =
            DependencyProperty<Color, SolidColorBrush>.Register("Color", new PropertyMetadata(Colors.White));

        public SolidColorBrush(Color color)
        {
            this.Color = color;
        }

        public Color Color
        {
            get
            {
                return this.GetValue<Color>(ColorProperty);
            }

            set
            {
                this.SetValue(ColorProperty, value);
            }
        }
    }
}