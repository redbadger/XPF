namespace RedBadger.Xpf.Data
{
    using System;

    internal interface IOneWayToSourceBinding<T> : IObserver<T>
    {
        IDisposable Initialize(IObservable<T> observable);
    }
}