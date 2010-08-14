namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using RedBadger.Xpf.Internal;

    public class Button : ContentControl
    {
        public static readonly XpfDependencyProperty PaddingProperty = XpfDependencyProperty.Register(
            "Padding", 
            typeof(Thickness), 
            typeof(Button), 
            new PropertyMetadata(new Thickness(), UIElementPropertyChangedCallbacks.PropertyOfTypeThickness));

        public Thickness Padding
        {
            get
            {
                return (Thickness)this.GetValue(PaddingProperty.Value);
            }

            set
            {
                this.SetValue(PaddingProperty.Value, value);
            }
        }

        public override void OnApplyTemplate()
        {
            if (this.Content != null)
            {
                this.Content.Margin = this.Padding;
            }
        }
    }
}