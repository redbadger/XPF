namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;

    public class DependencyProperty
    {
        public static readonly object UnsetValue = new object();

        private static readonly Dictionary<Type, Dictionary<string, DependencyProperty>> registeredProperties =
            new Dictionary<Type, Dictionary<string, DependencyProperty>>();

        private readonly string name;

        private readonly Type ownerType;

        private readonly Type propertyType;

        private object defaultValue;

        private Action<DependencyObject, DependencyPropertyChangedEventArgs> propertyChangedCallback;

        private DependencyProperty(string name, Type propertyType, Type ownerType)
        {
            this.name = name;
            this.propertyType = propertyType;
            this.ownerType = ownerType;
        }

        public Action<DependencyObject, DependencyPropertyChangedEventArgs> PropertyChangedCallback
        {
            get
            {
                return this.propertyChangedCallback;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public Type OwnerType
        {
            get
            {
                return this.ownerType;
            }
        }

        public Type PropertyType
        {
            get
            {
                return this.propertyType;
            }
        }

        public object DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }

        public static DependencyProperty Register(
            string name, Type propertyType, Type ownerType, PropertyMetadata propertyMetadata)
        {
            return Register(false, name, propertyType, ownerType, propertyMetadata);
        }

        public static DependencyProperty RegisterAttached(
            string name, Type propertyType, Type ownerType, PropertyMetadata propertyMetadata)
        {
            return Register(true, name, propertyType, ownerType, propertyMetadata);
        }

        internal static void StoreRegisteredProperty(string name, Type ownerType, DependencyProperty dependencyProperty)
        {
            Dictionary<string, DependencyProperty> properties;
            if (!registeredProperties.TryGetValue(ownerType, out properties))
            {
                properties = new Dictionary<string, DependencyProperty>();
                registeredProperties[ownerType] = properties;
            }

            properties[name] = dependencyProperty;
        }

        internal bool IsValidType(object value)
        {
            if (value == null)
            {
                if (this.propertyType.IsValueType &&
                    (!this.propertyType.IsGenericType ||
                     (this.propertyType.GetGenericTypeDefinition() != typeof(Nullable<>))))
                {
                    return false;
                }
            }
            else if (!this.propertyType.IsInstanceOfType(value))
            {
                return false;
            }

            return true;
        }

        private static DependencyProperty Register(
            bool isAttached, string name, Type propertyType, Type ownerType, PropertyMetadata propertyMetadata)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name cannot be an empty string");
            }

            if (propertyType == null)
            {
                throw new ArgumentNullException("propertyType");
            }

            if (ownerType == null)
            {
                throw new ArgumentNullException("ownerType");
            }

            var property = new DependencyProperty(name, propertyType, ownerType);

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
            else if (propertyType.IsValueType)
            {
                property.defaultValue = Activator.CreateInstance(propertyType);
            }

            StoreRegisteredProperty(name, ownerType, property);
            return property;
        }
    }
}