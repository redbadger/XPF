namespace RedBadger.Xpf.Data
{
    using System;
    using System.Globalization;
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
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            this.propertyInfo = propertyInfo;
        }

        public OneWayToSourceBinding(object source, PropertyInfo propertyInfo)
            : this(BindingResolutionMode.Immediate)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            this.observer = Observer.Create<T>(value => SetValue(source, propertyInfo, value));
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
            this.observer = Observer.Create<T>(value => SetValue(dataContext, this.propertyInfo, value));
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

        private static void SetValue(object source, PropertyInfo propertyInfo, object value)
        {
            if (value != null)
            {
                Type sourceType = propertyInfo.PropertyType;
                Type targetType = typeof(T);
                if (sourceType != targetType && !targetType.IsAssignableFrom(sourceType))
                {
                    value = Convert.ChangeType(value, sourceType, CultureInfo.InvariantCulture);
                }
            }

            propertyInfo.SetValue(source, value, null);
        }
    }
}