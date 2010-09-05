namespace RedBadger.Xpf.Presentation.Controls
{
    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Presentation.Controls.Primitives;

    public class Button : ButtonBase
    {
        public static readonly Property<Thickness, Button> PaddingProperty =
            Property<Thickness, Button>.Register(
                "Padding", new Thickness(), PropertyChangedCallbacks.InvalidateMeasure);

        public Thickness Padding
        {
            get
            {
                return this.GetValue(PaddingProperty);
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