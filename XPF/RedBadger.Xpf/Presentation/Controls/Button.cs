namespace RedBadger.Xpf.Presentation.Controls
{
    using RedBadger.Xpf.Presentation.Controls.Primitives;

    public class Button : ButtonBase
    {
        public static readonly ReactiveProperty<Thickness> PaddingProperty =
            ReactiveProperty<Thickness>.Register(
                "Padding", typeof(Button), new Thickness(), ReactivePropertyChangedCallbacks.InvalidateMeasure);

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