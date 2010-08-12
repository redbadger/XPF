namespace RedBadger.Xpf.Presentation.Media
{
    using System.Windows;

    using Microsoft.Xna.Framework;

    public class SolidColorBrush : Brush
    {
        public static readonly XpfDependencyProperty ColorProperty = XpfDependencyProperty.Register(
            "Color", typeof(Color), typeof(SolidColorBrush), new PropertyMetadata(Color.White));

        public SolidColorBrush(Color color)
        {
            this.Color = color;
        }

        public Color Color
        {
            get
            {
                return (Color)this.GetValue(ColorProperty.Value);
            }

            set
            {
                this.SetValue(ColorProperty.Value, value);
            }
        }
    }
}