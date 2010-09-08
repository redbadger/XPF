namespace RedBadger.Xpf.Presentation
{
    public static class ReactivePropertyChangedCallbacks
    {
        public static void InvalidateArrange<TProperty, TOwner>(TOwner owner, ReactivePropertyChangeEventArgs<TProperty, TOwner> args)
            where TOwner : class
        {
            var uiElement = owner as IElement;
            if (uiElement != null)
            {
                uiElement.InvalidateArrange();
            }
        }

        public static void InvalidateMeasure<TProperty, TOwner>(TOwner owner, ReactivePropertyChangeEventArgs<TProperty, TOwner> args)
            where TOwner : class
        {
            var uiElement = owner as IElement;
            if (uiElement != null)
            {
                uiElement.InvalidateMeasure();
            }
        }
    }
}