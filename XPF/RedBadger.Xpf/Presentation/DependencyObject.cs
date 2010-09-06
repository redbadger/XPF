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

        public IDisposable Bind<TProperty, TOwner>(Property<TProperty, TOwner> property, IObservable<TProperty> source)
            where TOwner : class
        {
            return source.Subscribe(this.GetSubject(property));
        }

        public IDisposable Bind<TProperty, TOwner>(Property<TProperty, TOwner> property, IObserver<TProperty> source)
            where TOwner : class
        {
            return this.GetSubject(property).Subscribe(source);
        }

        public IDisposable Bind<TProperty, TOwner>(
            Property<TProperty, TOwner> property, TwoWayBinding<TProperty> source) where TOwner : class
        {
            return this.Bind(property, source.Observable, source.Observer);
        }

        public IDisposable Bind<TProperty, TOwner>(
            Property<TProperty, TOwner> property, 
            IObservable<TProperty> sourceToTarget, 
            IObserver<TProperty> targetToSource) where TOwner : class
        {
            var target = this.GetSubject(property);

            var firstDisposable = sourceToTarget.Subscribe(target);
            var secondDisposable = target.Subscribe(targetToSource);

            return new DualDisposable(firstDisposable, secondDisposable);
        }

        public void ClearValue(IProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            this.propertyValues.Remove(property);
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

            var subject = this.GetSubject(property);
            TProperty oldValue = subject.First();

            if (Equals(newValue, Property<TProperty, TOwner>.UnsetValue))
            {
                this.ClearValue(property);
                this.RaiseChanged(property, oldValue, newValue);
            }
            else if (!Equals(newValue, oldValue))
            {
                subject.OnNext(newValue);
                this.RaiseChanged(property, oldValue, newValue);
            }
        }

        private ISubject<TProperty> GetSubject<TProperty, TOwner>(Property<TProperty, TOwner> property)
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

        private void RaiseChanged<TProperty, TOwner>(
            Property<TProperty, TOwner> property, TProperty oldValue, TProperty newValue) where TOwner : class
        {
            Action<PropertyChange<TProperty, TOwner>> changedCallback = property.ChangedCallback;
            if (changedCallback != null)
            {
                changedCallback(new PropertyChange<TProperty, TOwner>(this as TOwner, property, oldValue, newValue));
            }
        }
    }
}