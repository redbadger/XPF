namespace RedBadger.Xpf.Presentation.Media
{
    public class SolidColorBrush : Brush
    {
        public static readonly Property<Color, SolidColorBrush> ColorProperty =
            Property<Color, SolidColorBrush>.Register("Color", Colors.White);

        public SolidColorBrush(Color color)
        {
            this.Color = color;
        }

        public Color Color
        {
            get
            {
                return this.GetValue(ColorProperty);
            }

            set
            {
                this.SetValue(ColorProperty, value);
            }
        }
    }
}