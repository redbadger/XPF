namespace RedBadger.Xpf.Data
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    internal class TwoWayBinding<T> : IDualChannel<T>, IBinding, IDisposable
    {
        private readonly OneWayBinding<T> oneWayBinding;

        private readonly OneWayToSourceBinding<T> oneWayToSourceBinding;

        private readonly BindingResolutionMode resolutionMode;

        private bool isDisposed;

        public TwoWayBinding(PropertyInfo propertyInfo)
            : this(BindingResolutionMode.Deferred)
        {
            this.oneWayBinding = new OneWayBinding<T>(propertyInfo);
            this.oneWayToSourceBinding = new OneWayToSourceBinding<T>(propertyInfo);
        }

        public TwoWayBinding(object source, PropertyInfo propertyInfo)
            : this(BindingResolutionMode.Immediate)
        {
            this.oneWayBinding = new OneWayBinding<T>(source, propertyInfo);
            this.oneWayToSourceBinding = new OneWayToSourceBinding<T>(source, propertyInfo);
        }

        protected TwoWayBinding(
            OneWayBinding<T> oneWayBinding, 
            OneWayToSourceBinding<T> oneWayToSourceBinding, 
            BindingResolutionMode resolutionMode)
            : this(resolutionMode)
        {
            this.oneWayBinding = oneWayBinding;
            this.oneWayToSourceBinding = oneWayToSourceBinding;

            if (this.oneWayBinding.ResolutionMode != this.oneWayToSourceBinding.ResolutionMode)
            {
                throw new ArgumentException("Both bindings must share the same ResolutionMode");
            }
        }

        private TwoWayBinding(BindingResolutionMode resolutionMode)
        {
            this.resolutionMode = resolutionMode;
        }

        ~TwoWayBinding()
        {
            this.Dispose(false);
        }

        public IObservable<T> Observable
        {
            get
            {
                return this.oneWayBinding;
            }
        }

        public IObserver<T> Observer
        {
            get
            {
                return this.oneWayToSourceBinding;
            }
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
                    this.oneWayBinding.Dispose();
                    this.oneWayToSourceBinding.Dispose();
                }
            }

            this.isDisposed = true;
        }

        public IDisposable Initialize(ISubject<T> subject)
        {
            this.oneWayBinding.Subscribe(subject);
            this.oneWayToSourceBinding.Initialize(subject);
            return this;
        }

        public void Resolve(object dataContext)
        {
            this.oneWayBinding.Resolve(dataContext);
            this.oneWayToSourceBinding.Resolve(dataContext);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}