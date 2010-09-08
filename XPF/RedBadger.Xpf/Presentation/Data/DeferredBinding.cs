namespace RedBadger.Xpf.Presentation.Data
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    public class DeferredBinding<T> : IObservable<T>, IDeferredBinding
    {
        private readonly PropertyInfo propertyInfo;

        private readonly Subject<T> subject = new Subject<T>();

        private IDisposable subscription;

        public DeferredBinding(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        public void Resolve(object dataContext)
        {
            this.subscription.Dispose();
            this.subscription = BindingFactory.GetObservable<T>((INotifyPropertyChanged)dataContext, this.propertyInfo).Subscribe(this.subject);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return this.subject.Subscribe(observer);
        }
    }
}