namespace RedBadger.Xpf.Presentation.Data
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    internal class DeferredBinding<T> : IObservable<T>, IDeferredBinding, IDisposable
    {
        private readonly PropertyInfo propertyInfo;

        private readonly Subject<T> subject = new Subject<T>();

        private bool isDisposed;

        private IDisposable subscription;

        public DeferredBinding(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        ~DeferredBinding()
        {
            this.Dispose(false);
        }

        public void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                if (isDisposing)
                {
                    if (this.subscription != null)
                    {
                        this.subscription.Dispose();
                    }
                }
            }

            this.isDisposed = true;
        }

        public void Resolve(object dataContext)
        {
            this.subject.OnNext((T)this.propertyInfo.GetValue(dataContext, null));
            BindingFactory.GetObservable<T>((INotifyPropertyChanged)dataContext, this.propertyInfo).Subscribe(
                this.subject);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            this.subscription = this.subject.Subscribe(observer);
            return this;
        }
    }
}