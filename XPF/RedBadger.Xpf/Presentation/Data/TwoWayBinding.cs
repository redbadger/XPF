namespace RedBadger.Xpf.Presentation.Data
{
    using System;
    using System.Collections.Generic;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    internal class TwoWayBinding<T> : IDualChannel<T>, IBinding, IDisposable
    {
        private readonly OneWayBinding<T> oneWayBinding;

        private readonly OneWayToSourceBinding<T> oneWayToSourceBinding;

        private readonly BindingResolutionMode resolutionMode;

        private bool isDisposed;

        public TwoWayBinding(OneWayBinding<T> oneWayBinding, OneWayToSourceBinding<T> oneWayToSourceBinding)
        {
            this.oneWayBinding = oneWayBinding;
            this.oneWayToSourceBinding = oneWayToSourceBinding;

            if (this.oneWayBinding.ResolutionMode != this.oneWayToSourceBinding.ResolutionMode)
            {
                throw new ArgumentException("Both bindings must share the same ResolutionMode");
            }

            this.resolutionMode = this.oneWayBinding.ResolutionMode;
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

        public IDisposable Initialize(ISubject<T> subject)
        {
            this.oneWayBinding.Subscribe(subject);
            this.oneWayToSourceBinding.Initialize(subject);
            return this;
        }
    }
}