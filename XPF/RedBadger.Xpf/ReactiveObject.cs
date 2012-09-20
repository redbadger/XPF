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

namespace RedBadger.Xpf
{
#if WINDOWS_PHONE

	using System;
	using System.Collections.Generic;
	using System.Linq;

	using System.Reactive;
	using System.Reactive.Disposables;
	using System.Reactive.Linq;
	using System.Reactive.Subjects;

	using RedBadger.Xpf.Data;
	using RedBadger.Xpf.Internal;

#else
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    using RedBadger.Xpf.Data;
    using RedBadger.Xpf.Internal;
#endif

    /// <summary>
    ///     Represents an object that participates in the Reactive Property system.
    /// </summary>
    public class ReactiveObject : IReactiveObject
    {
        private readonly Dictionary<IReactiveProperty, IDisposable> propertryBindings =
            new Dictionary<IReactiveProperty, IDisposable>();

        private readonly Dictionary<IReactiveProperty, object> propertyValues =
            new Dictionary<IReactiveProperty, object>();

        /// <summary>
        ///     Bind One Way (from the Source).
        /// </summary>
        /// <typeparam name = "T">Target <see cref = "ReactiveProperty{T}">ReactiveProperty</see> <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <param name = "fromSource"><see cref = "IObservable{T}">IObservable</see> of updates from the source</param>
        public void Bind<T>(ReactiveProperty<T> property, IObservable<T> fromSource)
        {
            this.SetBinding(property, fromSource.Subscribe(this.GetSubject(property)));
        }

        /// <summary>
        ///     Bind One Way (to the Source).
        /// </summary>
        /// <typeparam name = "T">Target <see cref = "ReactiveProperty{T}">ReactiveProperty</see> <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <param name = "toSource"><see cref = "IObserver{T}">IObserver</see> of updates for the Source</param>
        public void Bind<T>(ReactiveProperty<T> property, IObserver<T> toSource)
        {
            var binding = toSource as IOneWayToSourceBinding<T>;

            IDisposable disposable = binding != null
                                         ? binding.Initialize(this.GetSubject(property))
                                         : this.GetSubject(property).Subscribe(toSource);

            this.SetBinding(property, disposable);
        }

        /// <summary>
        ///     Bind Two Way (from and to the Source)
        /// </summary>
        /// <typeparam name = "T">Target <see cref = "ReactiveProperty{T}">ReactiveProperty</see> <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <param name = "source">A <see cref = "TwoWayBinding{T}">TwoWayBinding</see> containing both an <see cref = "IObservable{T}">IObservable</see> and <see cref = "IObserver{T}">IObserver</see></param>
        public void Bind<T>(ReactiveProperty<T> property, IDualChannel<T> source)
        {
            var binding = source as TwoWayBinding<T>;

            if (binding != null)
            {
                this.SetBinding(property, binding.Initialize(this.GetSubject(property)));
            }
            else
            {
                this.Bind(property, source.Observable, source.Observer);
            }
        }

        /// <summary>
        ///     Bind Two Way (from and to the Source)
        /// </summary>
        /// <typeparam name = "T">Target <see cref = "ReactiveProperty{T}">ReactiveProperty</see> <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <param name = "fromSource"><see cref = "IObservable{T}">IObservable</see> of updates from the source</param>
        /// <param name = "toSource"><see cref = "IObserver{T}">IObserver</see> of updates for the Source</param>
        public void Bind<T>(ReactiveProperty<T> property, IObservable<T> fromSource, IObserver<T> toSource)
        {
            ISubject<T> target = this.GetSubject(property);
            this.SetBinding(property, new CompositeDisposable(fromSource.Subscribe(target), target.Subscribe(toSource)));
        }

        /// <summary>
        ///     Clears the binding on the specified property.
        /// </summary>
        /// <param name = "property">The property who's binding you want to clear.</param>
        public void ClearBinding(IReactiveProperty property)
        {
            IDisposable binding;
            if (this.propertryBindings.TryGetValue(property, out binding))
            {
                this.propertryBindings.Remove(property);
                binding.Dispose();
            }
        }

        public void ClearValue(IReactiveProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            this.propertyValues.Remove(property);

            this.ClearBinding(property);
        }

        public IObservable<T> GetObservable<T, TOwner>(ReactiveProperty<T> property)
            where TOwner : class, IReactiveObject
        {
            return this.GetSubject(property).AsObservable();
        }

        public IObserver<T> GetObserver<T, TOwner>(ReactiveProperty<T> property) where TOwner : class, IReactiveObject
        {
            return this.GetSubject(property).AsObserver();
        }

        public T GetValue<T>(ReactiveProperty<T> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            return this.GetSubject(property).First();
        }

        public void SetValue<T>(ReactiveProperty<T> property, T newValue)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            this.GetSubject(property).OnNext(newValue);
        }

        /// <summary>
        ///     Resolves all the deferred bindingss for this object using the Data Context.
        /// </summary>
        /// <param name = "dataContext">The Data Context against which the binding should be resolved.</param>
        protected void ResolveDeferredBindings(object dataContext)
        {
            this.propertryBindings.Values.OfType<IBinding>().Where(
                binding => binding.ResolutionMode == BindingResolutionMode.Deferred).ForEach(
                    deferredBinding => deferredBinding.Resolve(dataContext));
        }

        private ISubject<T> GetSubject<T>(ReactiveProperty<T> property)
        {
            object value;
            if (this.propertyValues.TryGetValue(property, out value))
            {
                return (ISubject<T>)value;
            }

            var subject = new ValueChangedBehaviorSubject<T>(property.DefaultValue);

            IObservable<T> leftSource = subject.StartWith(property.DefaultValue);
            IObservable<T> rightSource = leftSource.Skip(1);

            leftSource.Zip(
                rightSource, 
                (oldValue, newValue) => new ReactivePropertyChangeEventArgs<T>(property, oldValue, newValue)).Where(
                    propertyChange => !object.Equals(propertyChange.OldValue, propertyChange.NewValue)).Subscribe(
                        this.RaiseChanged);

            this.propertyValues.Add(property, subject);
            return subject;
        }

        private void RaiseChanged<T>(ReactivePropertyChangeEventArgs<T> reactivePropertyChange)
        {
            Action<IReactiveObject, ReactivePropertyChangeEventArgs<T>> changedCallback =
                reactivePropertyChange.Property.ChangedCallback;

            if (changedCallback != null)
            {
                changedCallback(this, reactivePropertyChange);
            }
        }

        private void SetBinding(IReactiveProperty property, IDisposable binding)
        {
            this.ClearBinding(property);
            this.propertryBindings[property] = binding;
        }
    }
}
