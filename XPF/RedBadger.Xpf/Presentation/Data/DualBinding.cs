namespace RedBadger.Xpf.Presentation.Data
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    public class DualBinding<T> : IDisposable, IObservable<T>, IObserver<T>
    {
        private readonly PropertyInfo propertyInfo;

        private readonly Subject<T> source = new Subject<T>();

        public DualBinding(IObserver<T> observer)
        {
            this.Subscribe(observer);
        }

        public DualBinding(IObservable<T> observable)
        {
            observable.Subscribe(this);
        }

        public DualBinding(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        public void Resolve(object dataContext)
        {
            /*this.observable.OnNext((T)this.propertyInfo.GetValue(dataContext, null));
            BindingFactory.GetObservable<T>((INotifyPropertyChanged)dataContext, this.propertyInfo).Subscribe(
                this.subject);*/
        }

        public void Dispose()
        {
            // TODO
        }

        public IDisposable Subscribe(IObserver<T> s)
        {
            return this.source.Subscribe(s);
        }

        public void OnNext(T value)
        {
            this.source.OnNext(value);
        }

        public void OnError(Exception error)
        {
            this.source.OnError(error);
        }

        public void OnCompleted()
        {
            this.source.OnCompleted();
        }
    }
}