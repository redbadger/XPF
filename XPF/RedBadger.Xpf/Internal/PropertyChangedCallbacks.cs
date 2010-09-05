namespace RedBadger.Xpf.Internal
{
    using RedBadger.Xpf.Presentation;

    internal static class PropertyChangedCallbacks
    {
        public static void InvalidateArrange<TProperty, TOwner>(
            TOwner owner, PropertyChangedEventArgs<TProperty, TOwner> args) where TOwner : class
        {
            if (!args.NewValue.Equals(args.OldValue))
            {
                var uiElement = owner as UIElement;
                if (uiElement != null)
                {
                    uiElement.InvalidateArrange();
                }
            }
        }

        public static void InvalidateMeasure<TProperty, TOwner>(
            TOwner owner, PropertyChangedEventArgs<TProperty, TOwner> args) where TOwner : class
        {
            if (!args.NewValue.Equals(args.OldValue))
            {
                var uiElement = owner as UIElement;
                if (uiElement != null)
                {
                    uiElement.InvalidateMeasure();
                }
            }
        }
    }
}