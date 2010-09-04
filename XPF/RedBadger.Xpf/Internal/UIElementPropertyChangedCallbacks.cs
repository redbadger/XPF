namespace RedBadger.Xpf.Internal
{
    using RedBadger.Xpf.Presentation;

    internal static class UIElementPropertyChangedCallbacks
    {
        public static void InvalidateMeasureIfThicknessChanged(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
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