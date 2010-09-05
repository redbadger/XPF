namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;

    public class DependencyObject : IDependencyObject
    {
        private readonly Dictionary<IProperty, object> propertyValues = new Dictionary<IProperty, object>();

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

            object value;
            if (this.propertyValues.TryGetValue(property, out value))
            {
                return (TProperty)value;
            }

            return (TProperty)property.DefaultValue;
        }

        public void SetValue<TProperty, TOwner>(Property<TProperty, TOwner> property, TProperty value)
            where TOwner : class
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            if (Equals(value, Property<TProperty, TOwner>.UnsetValue))
            {
                this.ClearValue(property);
            }
            else
            {
                TProperty oldValue = this.GetValue(property);
                if (!Equals(value, oldValue))
                {
                    this.propertyValues[property] = value;
                    if (property.ChangedCallback != null)
                    {
                        property.ChangedCallback(
                            this as TOwner, new PropertyChangedEventArgs<TProperty, TOwner>(property, oldValue, value));
                    }
                }
            }
        }
    }
}