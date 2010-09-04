namespace RedBadger.Xpf.Presentation.Controls
{
    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Presentation.Controls.Primitives;

    public class Button : ButtonBase
    {
        public static readonly IDependencyProperty PaddingProperty =
            DependencyProperty<Thickness, Button>.Register(
                "Padding", 
                new PropertyMetadata(new Thickness(), UIElementPropertyChangedCallbacks.InvalidateMeasureIfThicknessChanged));

        public Thickness Padding
        {
            get
            {
                return this.GetValue<Thickness>(PaddingProperty);
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