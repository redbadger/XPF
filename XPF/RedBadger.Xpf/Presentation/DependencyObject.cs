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
        private readonly Dictionary<IProperty, ISubject<object>> propertyValues =
            new Dictionary<IProperty, ISubject<object>>();

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

            return (TProperty)this.GetSubject(property).First();
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
                this.propertyValues[property].OnNext(newValue);
                this.RaiseChanged(property, oldValue, newValue);
            }
        }

        private ISubject<object> GetSubject<TProperty, TOwner>(Property<TProperty, TOwner> property)
            where TOwner : class
        {
            ISubject<object> value;
            if (this.propertyValues.TryGetValue(property, out value))
            {
                return value;
            }

            var subject = new BehaviorSubject<object>(property.DefaultValue);
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