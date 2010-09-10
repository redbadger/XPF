namespace RedBadger.Xpf.Presentation.Data
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    internal class Binding<T> : IObservable<T>, IBinding, IDisposable
    {
        private readonly IObservable<T> observable;

        private readonly PropertyInfo propertyInfo;

        private readonly BindingResolutionMode resolutionMode;

        private readonly Subject<T> subject = new Subject<T>();

        private bool isDisposed;

        private IObserver<T> observer;

        private IDisposable subscription;

        public Binding()
            : this(BindingResolutionMode.Deferred)
        {
        }

        public Binding(PropertyInfo propertyInfo)
            : this(BindingResolutionMode.Deferred)
        {
            this.propertyInfo = propertyInfo;
        }

        public Binding(object source, PropertyInfo propertyInfo)
            : this(BindingResolutionMode.Immediate)
        {
            var notifyPropertyChanged = source as INotifyPropertyChanged;

            this.observable = notifyPropertyChanged != null
                                  ? BindingFactory.GetObservable<T>(notifyPropertyChanged, propertyInfo)
                                  : new BehaviorSubject<T>((T)propertyInfo.GetValue(source, null));
        }

        protected Binding(BindingResolutionMode resolutionMode)
        {
            this.resolutionMode = resolutionMode;
        }

        protected Binding(IObservable<T> observable)
            : this(BindingResolutionMode.Immediate)
        {
            this.observable = observable;
        }

        ~Binding()
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

        protected Subject<T> Subject
        {
            get
            {
                return this.subject;
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
            this.SubscribeToObserver();

            if (this.propertyInfo == null)
            {
                this.subject.OnNext((T)dataContext);
            }
            else
            {
                this.subject.OnNext((T)this.propertyInfo.GetValue(dataContext, null));

                if (dataContext is INotifyPropertyChanged)
                {
                    BindingFactory.GetObservable<T>((INotifyPropertyChanged)dataContext, this.propertyInfo).Subscribe(
                        this.subject);
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
                this.subscription = this.observable.Subscribe(this.observer);
            }

            return this;
        }

        protected void SubscribeToObserver()
        {
            this.subscription = this.subject.Subscribe(this.observer);
        }
    }
}