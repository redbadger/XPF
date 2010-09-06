namespace RedBadger.Xpf.Internal
{
    using RedBadger.Xpf.Presentation;

    internal static class PropertyChangedCallbacks
    {
        public static void InvalidateArrange<TProperty, TOwner>(PropertyChange<TProperty, TOwner> args)
            where TOwner : class
        {
            var uiElement = args.Owner as UIElement;
            if (uiElement != null)
            {
                uiElement.InvalidateArrange();
            }
        }

        public static void InvalidateMeasure<TProperty, TOwner>(PropertyChange<TProperty, TOwner> args)
            where TOwner : class
        {
            var uiElement = args.Owner as UIElement;
            if (uiElement != null)
            {
                uiElement.InvalidateMeasure();
            }
        }
    }
}