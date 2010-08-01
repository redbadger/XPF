namespace RedBadger.Xpf.Media
{
    using System.Windows;

    using Microsoft.Xna.Framework;

    public class SolidColorBrush : Brush
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(SolidColorBrush), new PropertyMetadata(Color.White));

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