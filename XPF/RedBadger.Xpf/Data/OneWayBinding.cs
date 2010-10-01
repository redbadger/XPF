namespace RedBadger.Xpf.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    internal class OneWayBinding<T> : IObservable<T>, IBinding, IDisposable
    {
        private readonly T initialValue;

        private readonly PropertyInfo propertyInfo;

        private readonly BindingResolutionMode resolutionMode;

        private readonly bool shouldPushInitialValueOnSubscribe;

        private bool isDisposed;

        private IObservable<T> observable;

        private IObserver<T> observer;

        private IDisposable subscription;

        public OneWayBinding()
            : this(BindingResolutionMode.Deferred)
        {
        }

        public OneWayBinding(PropertyInfo propertyInfo)
            : this(BindingResolutionMode.Deferred)
        {
            this.propertyInfo = propertyInfo;
        }

        public OneWayBinding(T source)
            : this(BindingResolutionMode.Immediate)
        {
            this.observable = new BehaviorSubject<T>(source);
        }

        public OneWayBinding(object source, PropertyInfo propertyInfo)
            : this(BindingResolutionMode.Immediate)
        {
            this.initialValue = GetValue(source, propertyInfo);

            var notifyPropertyChanged = source as INotifyPropertyChanged;
            if (notifyPropertyChanged != null)
            {
                this.observable = GetObservable(notifyPropertyChanged, propertyInfo);
                this.shouldPushInitialValueOnSubscribe = true;
            }
            else
            {
                this.observable = new BehaviorSubject<T>(this.initialValue);
            }
        }

        protected OneWayBinding(BindingResolutionMode resolutionMode)
        {
            this.resolutionMode = resolutionMode;
        }

        protected OneWayBinding(IObservable<T> observable)
            : this(BindingResolutionMode.Immediate)
        {
            this.observable = observable;
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
            if (this.propertyInfo == null)
            {
                this.observer.OnNext((T)dataContext);
            }
            else
            {
                this.observer.OnNext(GetValue(dataContext, this.propertyInfo));

                if (dataContext is INotifyPropertyChanged)
                {
                    this.SubscribeObserver(GetObservable((INotifyPropertyChanged)dataContext, this.propertyInfo));
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

                this.SubscribeObserver();
            }

            return this;
        }

        protected void SubscribeObserver(IObservable<T> observable)
        {
            this.observable = observable;
            this.SubscribeObserver();
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
            object value = propertyInfo.GetValue(source, null);
            if (value != null)
            {
                var sourceType = value.GetType();
                var targetType = typeof(T);
                if (sourceType != targetType && !targetType.IsAssignableFrom(sourceType))
                {
                    value = Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
                }
            }

            return (T)value;
        }

        private void SubscribeObserver()
        {
            this.subscription = this.observable.Subscribe(this.observer);
        }
    }
}