namespace RedBadger.Xpf.Presentation.Controls
{
    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Presentation.Controls.Primitives;

    public class Button : ButtonBase
    {
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            "Padding", 
            typeof(Thickness), 
            typeof(Button), 
            new PropertyMetadata(new Thickness(), UIElementPropertyChangedCallbacks.InvalidateMeasureIfThicknessChanged));

        public Thickness Padding
        {
            get
            {
                return (Thickness)this.GetValue(PaddingProperty);
            }

            set
            {
                this.SetValue(PaddingProperty, value);
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