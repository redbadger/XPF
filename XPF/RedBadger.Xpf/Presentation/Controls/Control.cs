namespace RedBadger.Xpf.Presentation.Controls
{
    public class Control : UIElement
    {
        public static readonly Property<bool, Control> IsEnabledProperty =
            Property<bool, Control>.Register("IsEnabled", true);

        public bool IsEnabled
        {
            get
            {
                return this.GetValue(IsEnabledProperty);
            }

            set
            {
                this.SetValue(IsEnabledProperty, value);
            }
        }
    }
}