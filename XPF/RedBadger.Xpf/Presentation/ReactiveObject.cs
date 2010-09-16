namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Presentation.Data;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
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
        /// <typeparam name = "TProperty">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see></param>
        /// <param name = "fromSource"><see cref = "IObservable{T}">IObservable</see> of updates from the source</param>
        public void Bind<TProperty, TOwner>(
            ReactiveProperty<TProperty, TOwner> property, IObservable<TProperty> fromSource)
            where TOwner : class, IReactiveObject
        {
            this.SetBinding(property, fromSource.Subscribe(this.GetSubject(property)));
        }

        /// <summary>
        ///     Bind One Way (to the Source).
        /// </summary>
        /// <typeparam name = "TProperty">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see></param>
        /// <param name = "toSource"><see cref = "IObserver{T}">IObserver</see> of updates for the Source</param>
        public void Bind<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property, IObserver<TProperty> toSource)
            where TOwner : class, IReactiveObject
        {
            var binding = toSource as OneWayToSourceBinding<TProperty>;

            IDisposable disposable = binding != null
                                         ? binding.Initialize(this.GetSubject(property))
                                         : this.GetSubject(property).Subscribe(toSource);

            this.SetBinding(property, disposable);
        }

        /// <summary>
        ///     Bind Two Way (from and to the Source)
        /// </summary>
        /// <typeparam name = "TProperty">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see></param>
        /// <param name = "source">A <see cref = "TwoWayBinding{T}">TwoWayBinding</see> containing both an <see cref = "IObservable{T}">IObservable</see> and <see cref = "IObserver{T}">IObserver</see></param>
        public void Bind<TProperty, TOwner>(
            ReactiveProperty<TProperty, TOwner> property, IDualChannel<TProperty> source)
            where TOwner : class, IReactiveObject
        {
            var binding = source as TwoWayBinding<TProperty>;

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
        /// <typeparam name = "TProperty">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see></param>
        /// <param name = "fromSource"><see cref = "IObservable{T}">IObservable</see> of updates from the source</param>
        /// <param name = "toSource"><see cref = "IObserver{T}">IObserver</see> of updates for the Source</param>
        public void Bind<TProperty, TOwner>(
            ReactiveProperty<TProperty, TOwner> property, 
            IObservable<TProperty> fromSource, 
            IObserver<TProperty> toSource) where TOwner : class, IReactiveObject
        {
            ISubject<TProperty> target = this.GetSubject(property);
            this.SetBinding(property, new DualDisposable(fromSource.Subscribe(target), target.Subscribe(toSource)));
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

        public IObservable<TProperty> GetObservable<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property)
            where TOwner : class, IReactiveObject
        {
            return this.GetSubject(property).AsObservable();
        }

        public IObserver<TProperty> GetObserver<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property)
            where TOwner : class, IReactiveObject
        {
            return this.GetSubject(property).AsObserver();
        }

        public TProperty GetValue<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property)
            where TOwner : class, IReactiveObject
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            return this.GetSubject(property).First();
        }

        public void SetValue<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property, TProperty newValue)
            where TOwner : class, IReactiveObject
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            this.GetSubject(property).OnNext(newValue);
        }

        /// <summary>
        ///     Returns the nearest ancestor of the specified type, which maybe itself or null.
        /// </summary>
        /// <typeparam name = "T">The <see cref = "Type">Type</see> of the ancestor</typeparam>
        /// <returns>The nearest ancestor of Type T</returns>
        protected virtual T GetNearestAncestorOfType<T>() where T : class, IReactiveObject
        {
            return this as T;
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

        private ISubject<TProperty> GetSubject<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property)
            where TOwner : class, IReactiveObject
        {
            object value;
            if (this.propertyValues.TryGetValue(property, out value))
            {
                return (ISubject<TProperty>)value;
            }

            var subject = new ValueChangedBehaviorSubject<TProperty>(property.DefaultValue);

            IObservable<TProperty> leftSource = subject.StartWith(property.DefaultValue);
            IObservable<TProperty> rightSource = leftSource.Skip(1);

            leftSource.Zip(
                rightSource, 
                (oldValue, newValue) =>
                new ReactivePropertyChangeEventArgs<TProperty, TOwner>(property, oldValue, newValue)).Where(
                    propertyChange => !Equals(propertyChange.OldValue, propertyChange.NewValue)).Subscribe(
                        this.RaiseChanged);

            this.propertyValues.Add(property, subject);
            return subject;
        }

        private void RaiseChanged<TProperty, TOwner>(
            ReactivePropertyChangeEventArgs<TProperty, TOwner> reactivePropertyChange)
            where TOwner : class, IReactiveObject
        {
            Action<TOwner, ReactivePropertyChangeEventArgs<TProperty, TOwner>> changedCallback =
                reactivePropertyChange.Property.ChangedCallback;

            if (changedCallback != null)
            {
                changedCallback(this.GetNearestAncestorOfType<TOwner>(), reactivePropertyChange);
            }
        }

        private void SetBinding(IReactiveProperty property, IDisposable binding)
        {
            this.ClearBinding(property);
            this.propertryBindings[property] = binding;
        }
    }
}