namespace RedBadger.Xpf
{
    /// <summary>
    ///     Generic Methods to use as callbacks when a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> value is changed.
    /// </summary>
    public static class ReactivePropertyChangedCallbacks
    {
        /// <summary>
        ///     Invalidates the sender's Arrange layout pass when a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> value is changed.
        /// </summary>
        /// <typeparam name = "T">The type of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see>.</typeparam>
        /// <param name = "sender">The ReactiveObject whose <see cref = "ReactiveProperty{T}">ReactiveProperty</see> has changed.</param>
        /// <param name = "args">An instance of <see cref = "ReactivePropertyChangeEventArgs{T}">ReactivePropertyChangeEventArgs</see> that carries information about the change.</param>
        public static void InvalidateArrange<T>(IReactiveObject sender, ReactivePropertyChangeEventArgs<T> args)
        {
            var uiElement = sender as IElement;
            if (uiElement != null)
            {
                uiElement.InvalidateArrange();
            }
        }

        /// <summary>
        ///     Invalidates the sender's Measure layout pass when a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> value is changed.
        /// </summary>
        /// <typeparam name = "T">The type of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see>.</typeparam>
        /// <param name = "sender">The ReactiveObject whose <see cref = "ReactiveProperty{T}">ReactiveProperty</see> has changed.</param>
        /// <param name = "args">An instance of <see cref = "ReactivePropertyChangeEventArgs{T}">ReactivePropertyChangeEventArgs</see> that carries information about the change.</param>
        public static void InvalidateMeasure<T>(IReactiveObject sender, ReactivePropertyChangeEventArgs<T> args)
        {
            var uiElement = sender as IElement;
            if (uiElement != null)
            {
                uiElement.InvalidateMeasure();
            }
        }
    }
}