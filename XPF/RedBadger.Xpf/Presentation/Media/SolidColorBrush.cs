namespace RedBadger.Xpf.Presentation.Media
{
    public class SolidColorBrush : Brush
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(SolidColorBrush), new PropertyMetadata(Colors.White));

        public SolidColorBrush(Color color)
        {
            this.Color = color;
        }

        public Color Color
        {
            get
            {
                return (Color)this.GetValue(ColorProperty);
            }

            set
            {
                this.SetValue(ColorProperty, value);
            }
        }
    }
}