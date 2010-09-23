namespace RedBadger.Xpf.Controls
{
    public class Control : UIElement
    {
        public static readonly ReactiveProperty<bool> IsEnabledProperty = ReactiveProperty<bool>.Register(
            "IsEnabled", typeof(Control), true);

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