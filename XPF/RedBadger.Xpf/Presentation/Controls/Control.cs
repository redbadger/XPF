namespace RedBadger.Xpf.Presentation.Controls
{
    public class Control : UIElement
    {
        public static readonly ReactiveProperty<bool, Control> IsEnabledProperty =
            ReactiveProperty<bool, Control>.Register("IsEnabled", true);

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