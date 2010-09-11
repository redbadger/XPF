namespace RedBadger.Xpf.Presentation
{
    using System;

    /// <summary>
    ///     Represents an object that participates in the Reactive Property system.
    /// </summary>
    public interface IReactiveObject
    {
        /// <summary>
        ///     Clears the value of a <see cref = "IReactiveProperty">IReactiveProperty</see> on this instance of <see cref = "IReactiveObject">IReactiveObject</see>.
        /// </summary>
        /// <param name = "property">The <see cref = "IReactiveProperty">IReactiveProperty</see> to clear.</param>
        void ClearValue(IReactiveProperty property);

        /// <summary>
        ///     Gets the current value of a <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> on this instance of <see cref = "IReactiveObject">IReactiveObject</see>.
        /// </summary>
        /// <typeparam name = "TProperty">The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see>'s <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> to retrieve a value for.</param>
        /// <returns>The current value.</returns>
        TProperty GetValue<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property) where TOwner : class;

        /// <summary>
        ///     Sets the value of a <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> on this instance of <see cref = "IReactiveObject">IReactiveObject</see>.
        /// </summary>
        /// <typeparam name = "TProperty">The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see>'s <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> to set.</param>
        /// <param name = "newValue">The new value.</param>
        void SetValue<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property, TProperty newValue)
            where TOwner : class;
    }
}