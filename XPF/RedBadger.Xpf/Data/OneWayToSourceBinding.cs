namespace RedBadger.Xpf.Data
{
    using System;
    using System.Linq;
    using System.Reflection;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    internal class OneWayToSourceBinding<T> : IObserver<T>, IBinding, IDisposable
    {
        private readonly PropertyInfo propertyInfo;

        private readonly BindingResolutionMode resolutionMode;

        private bool isDisposed;

        private IObserver<T> observer;

        private IDisposable subscription;

        public OneWayToSourceBinding(PropertyInfo propertyInfo)
            : this(BindingResolutionMode.Deferred)
        {
            this.propertyInfo = propertyInfo;
        }

        public OneWayToSourceBinding(object source, PropertyInfo propertyInfo)
            : this(BindingResolutionMode.Immediate)
        {
            this.observer = Observer.Create<T>(value => propertyInfo.SetValue(source, value, null));
        }

        protected OneWayToSourceBinding(BindingResolutionMode resolutionMode)
        {
            this.resolutionMode = resolutionMode;
        }

        protected OneWayToSourceBinding(IObserver<T> observer)
            : this(BindingResolutionMode.Immediate)
        {
            this.observer = observer;
        }

        ~OneWayToSourceBinding()
        {
            this.Dispose(false);
        }

        public BindingResolutionMode ResolutionMode
        {
            get
            {
                return this.resolutionMode;
            }
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

        public IDisposable Initialize(IObservable<T> observable)
        {
            this.subscription = observable.Subscribe(this);
            return this;
        }

        public virtual void Resolve(object dataContext)
        {
            if (this.propertyInfo != null)
            {
                this.observer = Observer.Create<T>(value => this.propertyInfo.SetValue(dataContext, value, null));
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void OnCompleted()
        {
            if (this.observer != null)
            {
                this.observer.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            if (this.observer != null)
            {
                this.observer.OnError(error);
            }
        }

        public void OnNext(T value)
        {
            if (this.observer != null)
            {
                this.observer.OnNext(value);
            }
        }

        protected void SetObserver(IObserver<T> observer)
        {
            this.observer = observer;
        }
    }
}