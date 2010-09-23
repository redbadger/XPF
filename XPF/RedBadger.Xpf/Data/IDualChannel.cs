namespace RedBadger.Xpf.Data
{
    using System;

    /// <summary>
    ///     Represents two channels (an <see cref = "IObservable{T}">IObservable</see> and an <see cref = "IObserver{T}">IObserver</see>)
    /// </summary>
    /// <typeparam name = "T">The <see cref = "Type">Type</see> of the data in the channels</typeparam>
    public interface IDualChannel<T>
    {
        /// <summary>
        ///     The <see cref = "IObservable{T}">IObservable</see> channel
        /// </summary>
        IObservable<T> Observable { get; }

        /// <summary>
        ///     The <see cref = "IObserver{T}">IObserver</see> channel
        /// </summary>
        IObserver<T> Observer { get; }
    }
}