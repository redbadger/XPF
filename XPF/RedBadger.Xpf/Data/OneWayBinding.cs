namespace RedBadger.Xpf.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    internal class OneWayBinding<T> : IObservable<T>, IBinding, IDisposable
    {
        private readonly PropertyInfo propertyInfo;

        private readonly BindingResolutionMode resolutionMode;

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
            var notifyPropertyChanged = source as INotifyPropertyChanged;

            this.observable = notifyPropertyChanged != null
                                  ? GetObservable(notifyPropertyChanged, propertyInfo)
                                  : new BehaviorSubject<T>((T)propertyInfo.GetValue(source, null));
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
                this.observer.OnNext((T)this.propertyInfo.GetValue(dataContext, null));

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
                            e => (T)propertyInfo.GetValue(source, null));
        }

        private void SubscribeObserver()
        {
            this.subscription = this.observable.Subscribe(this.observer);
        }
    }
}