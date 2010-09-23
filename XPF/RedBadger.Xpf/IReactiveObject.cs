namespace RedBadger.Xpf
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
        ///     Gets an <see cref = "IObservable{T}">IObservable</see> for a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> for this instance of <see cref = "ReactiveObject">ReactiveObject</see>.
        /// </summary>
        /// <typeparam name = "T">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see>'s <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> to retrieve a value for.</param>
        /// <returns>An <see cref = "IObservable{T}">IObservable</see> around the <see cref = "ReactiveProperty{T}">ReactiveProperty</see>.</returns>
        IObservable<T> GetObservable<T, TOwner>(ReactiveProperty<T> property) where TOwner : class, IReactiveObject;

        /// <summary>
        ///     Gets an <see cref = "IObserver{T}">IObserver</see> for a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> for this instance of <see cref = "ReactiveObject">ReactiveObject</see>.
        /// </summary>
        /// <typeparam name = "T">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see>'s <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> to retrieve a value for.</param>
        /// <returns>An <see cref = "IObserver{T}">IObserver</see> around the <see cref = "ReactiveProperty{T}">ReactiveProperty</see>.</returns>
        IObserver<T> GetObserver<T, TOwner>(ReactiveProperty<T> property) where TOwner : class, IReactiveObject;

        /// <summary>
        ///     Gets the current value of a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on this instance of <see cref = "IReactiveObject">IReactiveObject</see>.
        /// </summary>
        /// <typeparam name = "T">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see>'s <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> to retrieve a value for.</param>
        /// <returns>The current value.</returns>
        T GetValue<T>(ReactiveProperty<T> property);

        /// <summary>
        ///     Sets the value of a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on this instance of <see cref = "IReactiveObject">IReactiveObject</see>.
        /// </summary>
        /// <typeparam name = "T">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see>'s <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> to set.</param>
        /// <param name = "newValue">The new value.</param>
        void SetValue<T>(ReactiveProperty<T> property, T newValue);
    }
}