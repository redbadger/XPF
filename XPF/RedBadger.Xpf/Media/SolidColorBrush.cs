namespace RedBadger.Xpf.Media
{
    public class SolidColorBrush : Brush
    {
        public static readonly ReactiveProperty<Color> ColorProperty = ReactiveProperty<Color>.Register(
            "Color", typeof(SolidColorBrush), Colors.White);

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