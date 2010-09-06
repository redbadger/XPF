namespace RedBadger.Xpf.Presentation.Data
{
    using System;

    public struct TwoWayBinding<T>
    {
        public TwoWayBinding(IObservable<T> observable, IObserver<T> observer)
            : this()
        {
            this.Observable = observable;
            this.Observer = observer;
        }

        public IObservable<T> Observable { get; set; }

        public IObserver<T> Observer { get; set; }
    }
}