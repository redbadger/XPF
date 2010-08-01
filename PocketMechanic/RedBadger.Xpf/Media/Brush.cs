namespace RedBadger.Xpf.Media
{
    using System.Windows;

    public abstract class Brush : DependencyObject
    {
        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register(
            "Opacity", typeof(float), typeof(Brush), new PropertyMetadata(1f));

        public float Opacity
        {
            get
            {
                return (float)this.GetValue(OpacityProperty);
            }

            set
            {
                this.SetValue(OpacityProperty, value);
            }
        }
    }
}