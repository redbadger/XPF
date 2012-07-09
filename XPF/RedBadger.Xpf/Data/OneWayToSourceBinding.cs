namespace RedBadger.Xpf.Data
{
    using System;
    using System.Globalization;
    using System.Reflection;

    using System.Reactive;
    using System.Reactive.Linq;

    internal class OneWayToSourceBinding<T> : IOneWayToSourceBinding<T>, IBinding, IDisposable
    {
        private readonly PropertyInfo propertyInfo;

        private readonly BindingResolutionMode resolutionMode;

        private bool isDisposed;

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

            this.TargetObserver = Observer.Create<T>(value => SetValue(source, propertyInfo, value));
        }

        protected OneWayToSourceBinding(BindingResolutionMode resolutionMode)
        {
            this.resolutionMode = resolutionMode;
        }

        protected OneWayToSourceBinding(IObserver<T> targetObserver)
            : this(BindingResolutionMode.Immediate)
        {
            this.TargetObserver = targetObserver;
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

        protected T InitialValue { get; private set; }

        protected IDisposable Subscription { get; set; }

        protected IObserver<T> TargetObserver { get; set; }

        public void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                if (isDisposing)
                {
                    if (this.Subscription != null)
                    {
                        this.Subscription.Dispose();
                    }
                }
            }

            this.isDisposed = true;
        }

        public virtual void Resolve(object dataContext)
        {
            this.TargetObserver = Observer.Create<T>(value => SetValue(dataContext, this.propertyInfo, value));
            this.OnNext(this.InitialValue);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void OnCompleted()
        {
            if (this.TargetObserver != null)
            {
                this.TargetObserver.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            if (this.TargetObserver != null)
            {
                this.TargetObserver.OnError(error);
            }
        }

        public void OnNext(T value)
        {
            if (this.TargetObserver != null)
            {
                this.TargetObserver.OnNext(value);
            }
        }

        public virtual IDisposable Initialize(IObservable<T> observable)
        {
            this.Subscription = observable.Subscribe(this);
            this.InitialValue = observable.FirstOrDefault();
            return this;
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