namespace RedBadger.Xpf.Presentation.Controls
{
    public class Control : UIElement
    {
        public static readonly IDependencyProperty IsEnabledProperty =
            DependencyProperty<bool, Control>.Register("IsEnabled", new PropertyMetadata(true));

        public bool IsEnabled
        {
            get
            {
                return this.GetValue<bool>(IsEnabledProperty);
            }

            set
            {
                this.SetValue(IsEnabledProperty, value);
            }
        }
    }
}