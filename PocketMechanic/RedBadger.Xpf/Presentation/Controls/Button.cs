namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Windows;

    using RedBadger.Xpf.Internal;

    using Size = RedBadger.Xpf.Presentation.Size;
    using Thickness = RedBadger.Xpf.Presentation.Thickness;
    using UIElement = RedBadger.Xpf.Presentation.UIElement;

    public class Button : ContentControl
    {
        public static readonly XpfDependencyProperty PaddingProperty = XpfDependencyProperty.Register(
            "Padding",
            typeof(Thickness),
            typeof(Button),
            new PropertyMetadata(Thickness.Empty, UIElementPropertyChangedCallbacks.PropertyOfTypeThickness));

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

        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(availableSize) + this.Padding.Collapse();
        }
    }
}