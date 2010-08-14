namespace RedBadger.Xpf.Internal
{
    using System.Windows;

    using UIElement = RedBadger.Xpf.Presentation.UIElement;

    internal static class UIElementPropertyChangedCallbacks
    {
        public static void PropertyOfTypeThickness(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var newValue = (Thickness)args.NewValue;
            var oldValue = (Thickness)args.OldValue;

            if (newValue.IsDifferentFrom(oldValue))
            {
                var uiElement = dependencyObject as UIElement;
                if (uiElement != null)
                {
                    uiElement.InvalidateMeasure();
                }
            }
        }
    }
}