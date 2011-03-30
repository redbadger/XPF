namespace RedBadger.Xpf.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    internal class OneWayBinding<T> : IObservable<T>, IBinding, IDisposable
    {
        private readonly PropertyInfo deferredProperty;

        private readonly T initialValue;

        private readonly BindingResolutionMode resolutionMode;

        private readonly bool shouldPushInitialValueOnSubscribe;

        private INotifyPropertyChanged deferredSource;

        private bool isDisposed;

        private IObserver<T> observer;

        private IObservable<T> sourceObservable;

        private IDisposable subscription;

        /// <summary>
        ///     A one-way binding to the data context
        /// </summary>
        public OneWayBinding()
            : this(BindingResolutionMode.Deferred)
        {
        }

        /// <summary>
        ///     A one-way binding to a property on the data context
        /// </summary>
        /// <param name = "propertyInfo"></param>
        public OneWayBinding(PropertyInfo propertyInfo)
            : this(BindingResolutionMode.Deferred)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            this.deferredProperty = propertyInfo;
            this.sourceObservable = Observable.Defer(this.GetDeferredObservable);
        }

        /// <summary>
        ///     A one-way binding to a specified source
        /// </summary>
        /// <param name = "source"></param>
        public OneWayBinding(T source)
            : this(BindingResolutionMode.Immediate)
        {
            this.sourceObservable = new BehaviorSubject<T>(source);
        }

        /// <summary>
        ///     A one-way binding to a property on a specified source
        /// </summary>
        /// <param name = "source"></param>
        /// <param name = "propertyInfo"></param>
        public OneWayBinding(object source, PropertyInfo propertyInfo)
            : this(BindingResolutionMode.Immediate)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            this.initialValue = GetValue(source, propertyInfo);

            var notifyPropertyChanged = source as INotifyPropertyChanged;
            if (notifyPropertyChanged != null)
            {
                this.sourceObservable = GetObservable(notifyPropertyChanged, propertyInfo);
                this.shouldPushInitialValueOnSubscribe = true;
            }
            else
            {
                this.sourceObservable = new BehaviorSubject<T>(this.initialValue);
            }
        }

        protected OneWayBinding(BindingResolutionMode resolutionMode)
        {
            this.resolutionMode = resolutionMode;
        }

        ~OneWayBinding()
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

        protected IObserver<T> Observer
        {
            get
            {
                return this.observer;
            }
        }

        protected IObservable<T> SourceObservable
        {
            get
            {
                return this.sourceObservable;
            }

            set
            {
                this.sourceObservable = value;
            }
        }

        protected IDisposable Subscription
        {
            set
            {
                this.subscription = value;
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

        public virtual void Resolve(object dataContext)
        {
            if (this.sourceObservable == null)
            {
                this.observer.OnNext(Convert(dataContext));
            }
            else
            {
                this.observer.OnNext(GetValue(dataContext, this.deferredProperty));

                if (dataContext is INotifyPropertyChanged)
                {
                    this.deferredSource = (INotifyPropertyChanged)dataContext;
                    this.subscription = this.sourceObservable.Subscribe(this.observer);
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            this.observer = observer;

            if (this.resolutionMode == BindingResolutionMode.Immediate)
            {
                if (this.shouldPushInitialValueOnSubscribe)
                {
                    this.observer.OnNext(this.initialValue);
                }

                this.subscription = this.sourceObservable.Subscribe(this.observer);
            }

            return this;
        }

        internal static T Convert(object value)
        {
            if (value != null)
            {
                Type sourceType = value.GetType();
                Type targetType = typeof(T);
                if (sourceType != targetType && !targetType.IsAssignableFrom(sourceType))
                {
                    value = System.Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
                }
            }

            return (T)value;
        }

        private static IObservable<T> GetObservable(INotifyPropertyChanged source, PropertyInfo propertyInfo)
        {
            return
                Observable.FromEvent<PropertyChangedEventArgs>(
                    handler => source.PropertyChanged += handler, handler => source.PropertyChanged -= handler).Where(
                        data => data.EventArgs.PropertyName == propertyInfo.Name).Select(
                            e => GetValue(source, propertyInfo));
        }

        private static T GetValue(object source, PropertyInfo propertyInfo)
        {
            return Convert(propertyInfo.GetValue(source, null));
        }

        private IObservable<T> GetDeferredObservable()
        {
            return GetObservable(this.deferredSource, this.deferredProperty);
        }
    }
}