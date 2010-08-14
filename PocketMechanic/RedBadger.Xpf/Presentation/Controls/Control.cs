namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using UIElement = RedBadger.Xpf.Presentation.UIElement;

    public class Control : UIElement
    {
        public static readonly XpfDependencyProperty IsEnabledProperty = XpfDependencyProperty.Register(
            "IsEnabled", typeof(bool), typeof(Control), new PropertyMetadata(true));

        public bool IsEnabled
        {
            get
            {
                return (bool)this.GetValue(IsEnabledProperty.Value);
            }

            set
            {
                this.SetValue(IsEnabledProperty.Value, value);
            }
        }
    }
}