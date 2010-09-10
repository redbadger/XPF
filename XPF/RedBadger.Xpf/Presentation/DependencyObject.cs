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

    public class DependencyObject : IDependencyObject
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
            ReactiveProperty<TProperty, TOwner> property, IObservable<TProperty> fromSource) where TOwner : class
        {
            this.SetBinding(property, fromSource.Subscribe(this.GetSubject(property)));
        }

        /// <summary>
        ///     Bind One Way (to a default source eg. the Target's DataContext).
        /// </summary>
        /// <typeparam name = "TProperty">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see></param>
        public virtual void Bind<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property) where TOwner : class
            where TProperty : class
        {
            throw new NotImplementedException("Derrived classes should provide a default implementation.");
        }

        /// <summary>
        ///     Bind One Way (to the Source).
        /// </summary>
        /// <typeparam name = "TProperty">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see></param>
        /// <param name = "toSource"><see cref = "IObserver{T}">IObserver</see> of updates for the Source</param>
        public void Bind<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property, IObserver<TProperty> toSource)
            where TOwner : class
        {
            this.SetBinding(property, this.GetSubject(property).Subscribe(toSource));
        }

        /// <summary>
        ///     Bind Two Way (from and to the Source)
        /// </summary>
        /// <typeparam name = "TProperty">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see></param>
        /// <param name = "source">A <see cref = "TwoWayBinding{T}">TwoWayBinding</see> containing both an <see cref = "IObservable{T}">IObservable</see> and <see cref = "IObserver{T}">IObserver</see></param>
        public void Bind<TProperty, TOwner>(
            ReactiveProperty<TProperty, TOwner> property, TwoWayBinding<TProperty> source) where TOwner : class
        {
            this.Bind(property, source.Observable, source.Observer);
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
            IObserver<TProperty> toSource) where TOwner : class
        {
            ISubject<TProperty> target = this.GetSubject(property);
            this.SetBinding(property, new DualDisposable(fromSource.Subscribe(target), target.Subscribe(toSource)));
        }

        /// <summary>
        ///     Clears the binding on the specified property.
        /// </summary>
        /// <typeparam name = "TProperty">The type of the property.</typeparam>
        /// <typeparam name = "TOwner">The type of the owner.</typeparam>
        /// <param name = "property">The property who's binding you want to clear.</param>
        public void ClearBinding<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property) where TOwner : class
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
        }

        public IObservable<TProperty> GetObservable<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property)
            where TOwner : class
        {
            return this.GetSubject(property).AsObservable();
        }

        public TProperty GetValue<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property) where TOwner : class
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            return this.GetSubject(property).First();
        }

        public void SetValue<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property, TProperty newValue)
            where TOwner : class
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            this.GetSubject(property).OnNext(newValue);
        }

        protected internal IObserver<TProperty> GetObserver<TProperty, TOwner>(
            ReactiveProperty<TProperty, TOwner> property) where TOwner : class
        {
            return this.GetSubject(property).AsObserver();
        }

        /// <summary>
        ///     Gets all bindings for this object that implement <see cref = "IDeferredBinding">IDeferredBinding</see>.
        /// </summary>
        /// <returns>An Enumerable of <see cref = "IDeferredBinding">IDeferredBinding</see>.</returns>
        protected IEnumerable<IDeferredBinding> GetDeferredBindings()
        {
            return this.propertryBindings.Values.OfType<IDeferredBinding>();
        }

        /// <summary>
        ///     Returns the nearest ancestor of the specified type, which maybe itself or null.
        /// </summary>
        /// <typeparam name = "T">The <see cref = "Type">Type</see> of the ancestor</typeparam>
        /// <returns>The nearest ancestor of Type T</returns>
        protected virtual T GetNearestAncestorOfType<T>() where T : class
        {
            return this as T;
        }

        protected ISubject<TProperty> GetSubject<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property)
            where TOwner : class
        {
            object value;
            if (this.propertyValues.TryGetValue(property, out value))
            {
                return (ISubject<TProperty>)value;
            }

            var subject = new BehaviorSubject<TProperty>(property.DefaultValue);

            var leftSource = subject.StartWith(property.DefaultValue);
            var rightSource = leftSource.Skip(1);

            leftSource.Zip(
                rightSource, 
                (oldValue, newValue) =>
                new ReactivePropertyChangeEventArgs<TProperty, TOwner>(property, oldValue, newValue)).Where(
                    propertyChange => !Equals(propertyChange.OldValue, propertyChange.NewValue)).Subscribe(
                        this.RaiseChanged);

            this.propertyValues.Add(property, subject);
            return subject;
        }

        protected void SetBinding<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property, IDisposable binding)
            where TOwner : class
        {
            this.ClearBinding(property);
            this.propertryBindings[property] = binding;
        }

        private void RaiseChanged<TProperty, TOwner>(
            ReactivePropertyChangeEventArgs<TProperty, TOwner> reactivePropertyChange) where TOwner : class
        {
            Action<TOwner, ReactivePropertyChangeEventArgs<TProperty, TOwner>> changedCallback =
                reactivePropertyChange.Property.ChangedCallback;

            if (changedCallback != null)
            {
                changedCallback(this.GetNearestAncestorOfType<TOwner>(), reactivePropertyChange);
            }
        }
    }
}