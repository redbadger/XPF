namespace RedBadger.Xpf.Presentation.Controls
{
    public class Control : UIElement
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            "IsEnabled", typeof(bool), typeof(Control), new PropertyMetadata(true));

        public bool IsEnabled
        {
            get
            {
                return (bool)this.GetValue(IsEnabledProperty);
            }

            set
            {
                this.SetValue(IsEnabledProperty, value);
            }
        }
    }
}