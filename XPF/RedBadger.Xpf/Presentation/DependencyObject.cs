namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    public class DependencyObject : IDependencyObject
    {
        private readonly Dictionary<IProperty, object> propertyValues = new Dictionary<IProperty, object>();

        public IDisposable Bind<TProperty, TOwner>(
            Property<TProperty, TOwner> property, IObservable<TProperty> observable) where TOwner : class
        {
            return observable.Subscribe(this.GetSubject(property));
        }

        public IDisposable Bind<TProperty, TOwner>(
            Property<TProperty, TOwner> property, IObserver<TProperty> observable) where TOwner : class
        {
            return this.GetSubject(property).Subscribe(observable);
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

            TProperty oldValue = this.GetValue(property);

            if (Equals(newValue, Property<TProperty, TOwner>.UnsetValue))
            {
                this.ClearValue(property);
                this.RaiseChanged(property, oldValue, newValue);
            }
            else if (!Equals(newValue, oldValue))
            {
                this.GetSubject(property).OnNext(newValue);
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