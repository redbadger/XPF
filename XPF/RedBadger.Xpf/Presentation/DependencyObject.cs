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
        private readonly Dictionary<IProperty, object> propertyValues = new Dictionary<IProperty, object>();

        /// <summary>
        ///     Bind One Way (from the Source).
        /// </summary>
        /// <typeparam name = "TProperty">Target <see cref = "Property{TProperty,TOwner}">Property</see> <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">Target <see cref = "Property{TProperty,TOwner}">Property</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "Property{TProperty,TOwner}">Property</see></param>
        /// <param name = "fromSource"><see cref = "IObservable{T}">IObservable</see> of updates from the source</param>
        /// <returns>A <see cref = "IDisposable">Disposable</see> subscription.</returns>
        public IDisposable Bind<TProperty, TOwner>(
            Property<TProperty, TOwner> property, IObservable<TProperty> fromSource) where TOwner : class
        {
            ISubject<TProperty> target = this.GetSubject(property);
            TProperty oldValue = target.First();
            IDisposable sourceSubscription = fromSource.Subscribe(target);
            TProperty newValue = target.First();

            if (!Equals(newValue, oldValue))
            {
                this.RaiseChanged(property, oldValue, newValue);
            }

            return sourceSubscription;
        }

        /// <summary>
        ///     Bind One Way (to a default source eg. the Target's DataContext).
        /// </summary>
        /// <typeparam name = "TProperty">Target <see cref = "Property{TProperty,TOwner}">Property</see> <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">Target <see cref = "Property{TProperty,TOwner}">Property</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "Property{TProperty,TOwner}">Property</see></param>
        /// <returns>A <see cref = "IDisposable">Disposable</see> subscription.</returns>
        public virtual IDisposable Bind<TProperty, TOwner>(Property<TProperty, TOwner> property) where TOwner : class
            where TProperty : class
        {
            throw new NotImplementedException("Derrived classes should provide a default implementation.");
        }

        /// <summary>
        ///     Bind One Way (to the Source).
        /// </summary>
        /// <typeparam name = "TProperty">Target <see cref = "Property{TProperty,TOwner}">Property</see> <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">Target <see cref = "Property{TProperty,TOwner}">Property</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "Property{TProperty,TOwner}">Property</see></param>
        /// <param name = "toSource"><see cref = "IObserver{T}">IObserver</see> of updates for the Source</param>
        /// <returns>A <see cref = "IDisposable">Disposable</see> subscription.</returns>
        public IDisposable Bind<TProperty, TOwner>(Property<TProperty, TOwner> property, IObserver<TProperty> toSource)
            where TOwner : class
        {
            return this.GetSubject(property).Subscribe(toSource);
        }

        /// <summary>
        ///     Bind Two Way (from and to the Source)
        /// </summary>
        /// <typeparam name = "TProperty">Target <see cref = "Property{TProperty,TOwner}">Property</see> <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">Target <see cref = "Property{TProperty,TOwner}">Property</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "Property{TProperty,TOwner}">Property</see></param>
        /// <param name = "source">A <see cref = "TwoWayBinding{T}">TwoWayBinding</see> containing both an <see cref = "IObservable{T}">IObservable</see> and <see cref = "IObserver{T}">IObserver</see></param>
        /// <returns>A <see cref = "IDisposable">Disposable</see> subscription.</returns>
        public IDisposable Bind<TProperty, TOwner>(
            Property<TProperty, TOwner> property, TwoWayBinding<TProperty> source) where TOwner : class
        {
            return this.Bind(property, source.Observable, source.Observer);
        }

        /// <summary>
        ///     Bind Two Way (from and to the Source)
        /// </summary>
        /// <typeparam name = "TProperty">Target <see cref = "Property{TProperty,TOwner}">Property</see> <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">Target <see cref = "Property{TProperty,TOwner}">Property</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">Target <see cref = "Property{TProperty,TOwner}">Property</see></param>
        /// <param name = "fromSource"><see cref = "IObservable{T}">IObservable</see> of updates from the source</param>
        /// <param name = "toSource"><see cref = "IObserver{T}">IObserver</see> of updates for the Source</param>
        /// <returns>A <see cref = "IDisposable">Disposable</see> subscription.</returns>
        public IDisposable Bind<TProperty, TOwner>(
            Property<TProperty, TOwner> property, IObservable<TProperty> fromSource, IObserver<TProperty> toSource)
            where TOwner : class
        {
            ISubject<TProperty> target = this.GetSubject(property);

            TProperty oldValue = target.First();
            IDisposable sourceSubscription = fromSource.Subscribe(target);
            TProperty newValue = target.First();

            if (!Equals(newValue, oldValue))
            {
                this.RaiseChanged(property, oldValue, newValue);
            }

            IDisposable targetSubscription = target.Subscribe(toSource);

            return new DualDisposable(sourceSubscription, targetSubscription);
        }

        public void ClearValue(IProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            this.propertyValues.Remove(property);
        }

        public IObservable<TProperty> GetObservable<TProperty, TOwner>(Property<TProperty, TOwner> property)
            where TOwner : class
        {
            return this.GetSubject(property).AsObservable();
        }

        public TProperty GetValue<TProperty, TOwner>(Property<TProperty, TOwner> property) where TOwner : class
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            return this.GetSubject(property).First();
        }

        public void SetValue<TProperty, TOwner>(Property<TProperty, TOwner> property, TProperty newValue)
            where TOwner : class
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            ISubject<TProperty> subject = this.GetSubject(property);
            TProperty oldValue = subject.First();

            if (!Equals(newValue, oldValue))
            {
                subject.OnNext(newValue);
                this.RaiseChanged(property, oldValue, newValue);
            }
        }

        protected internal IObserver<TProperty> GetObserver<TProperty, TOwner>(Property<TProperty, TOwner> property)
            where TOwner : class
        {
            return this.GetSubject(property).AsObserver();
        }

        protected ISubject<TProperty> GetSubject<TProperty, TOwner>(Property<TProperty, TOwner> property)
            where TOwner : class
        {
            object value;
            if (this.propertyValues.TryGetValue(property, out value))
            {
                return (ISubject<TProperty>)value;
            }

            var subject = new BehaviorSubject<TProperty>(property.DefaultValue);
            this.propertyValues.Add(property, subject);
            return subject;
        }

        protected virtual void RaiseChanged<TProperty, TOwner>(
            Property<TProperty, TOwner> property, TProperty oldValue, TProperty newValue) where TOwner : class
        {
        }
    }
}