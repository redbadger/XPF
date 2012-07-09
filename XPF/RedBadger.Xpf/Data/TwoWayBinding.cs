#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

namespace RedBadger.Xpf.Data
{
    using System;
    using System.Reactive.Subjects;
    using System.Reflection;

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
