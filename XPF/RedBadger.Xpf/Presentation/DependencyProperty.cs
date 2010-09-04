namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;

    public class DependencyProperty<TProperty, TOwner> : IDependencyProperty
    {
        public static readonly object UnsetValue = new object();

        private static readonly DependencyPropertyStore<TProperty, TOwner> registeredProperties =
            new DependencyPropertyStore<TProperty, TOwner>();

        private readonly string name;

        private object defaultValue;

        private Action<DependencyObject, DependencyPropertyChangedEventArgs> propertyChangedCallback;

        private DependencyProperty(string name)
        {
            this.name = name;
        }

        public object DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public Action<DependencyObject, DependencyPropertyChangedEventArgs> PropertyChangedCallback
        {
            get
            {
                return this.propertyChangedCallback;
            }
        }

        public Type PropertyType
        {
            get
            {
                return typeof(TProperty);
            }
        }

        public static DependencyProperty<TProperty, TOwner> Register(string name, PropertyMetadata propertyMetadata)
        {
            return Register(false, name, propertyMetadata);
        }

        public static DependencyProperty<TProperty, TOwner> RegisterAttached(
            string name, PropertyMetadata propertyMetadata)
        {
            return Register(true, name, propertyMetadata);
        }

        internal static void StoreRegisteredProperty(
            string name, DependencyProperty<TProperty, TOwner> dependencyProperty)
        {
            Dictionary<string, DependencyProperty<TProperty, TOwner>> properties;
            if (!registeredProperties.TryGetValue(typeof(TOwner), out properties))
            {
                properties = new Dictionary<string, DependencyProperty<TProperty, TOwner>>();
                registeredProperties[typeof(TOwner)] = properties;
            }

            properties[name] = dependencyProperty;
        }

        internal bool IsValidType(object value)
        {
            if (value == null)
            {
                if (typeof(TProperty).IsValueType &&
                    (!typeof(TProperty).IsGenericType ||
                     (typeof(TProperty).GetGenericTypeDefinition() != typeof(Nullable<>))))
                {
                    return false;
                }
            }
            else if (!typeof(TProperty).IsInstanceOfType(value))
            {
                return false;
            }

            return true;
        }

        private static DependencyProperty<TProperty, TOwner> Register(
            bool isAttached, string name, PropertyMetadata propertyMetadata)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name cannot be an empty string");
            }

            var property = new DependencyProperty<TProperty, TOwner>(name);

            if (propertyMetadata != null)
            {
                property.propertyChangedCallback = propertyMetadata.PropertyChangedCallback;
            }

            if (propertyMetadata != null)
            {
                if (!property.IsValidType(propertyMetadata.DefaultValue) && propertyMetadata.DefaultValue != UnsetValue)
                {
                    throw new ArgumentException("The default value does not match the property type");
                }

                property.defaultValue = propertyMetadata.DefaultValue;
            }
            else if (typeof(TProperty).IsValueType)
            {
                property.defaultValue = default(TProperty);
            }

            StoreRegisteredProperty(name, property);
            return property;
        }
    }
}