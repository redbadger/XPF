namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;

    using RedBadger.Xpf.Internal;

    public class Property<TProperty, TOwner> : IProperty
        where TOwner : class
    {
        private static readonly PropertyStore<TProperty, TOwner> registeredProperties =
            new PropertyStore<TProperty, TOwner>();

        private readonly Action<PropertyChange<TProperty, TOwner>> changedCallback;

        private readonly TProperty defaultValue;

        private readonly string name;

        private Property(string name, TProperty defaultValue, Action<PropertyChange<TProperty, TOwner>> changedCallback)
        {
            this.name = name;
            this.defaultValue = defaultValue;
            this.changedCallback = changedCallback;
        }

        public Action<PropertyChange<TProperty, TOwner>> ChangedCallback
        {
            get
            {
                return this.changedCallback;
            }
        }

        public TProperty DefaultValue
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

        public static Property<TProperty, TOwner> Register(string propertyName)
        {
            return Register(propertyName, default(TProperty));
        }

        public static Property<TProperty, TOwner> Register(string propertyName, TProperty defaultValue)
        {
            return Register(propertyName, defaultValue, null);
        }

        public static Property<TProperty, TOwner> Register(
            string propertyName, TProperty defaultValue, Action<PropertyChange<TProperty, TOwner>> changedCallback)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("propertyName cannot be null or an empty string");
            }

            var property = new Property<TProperty, TOwner>(propertyName, defaultValue, changedCallback);

            StoreRegisteredProperty(propertyName, property);
            return property;
        }

        private static void StoreRegisteredProperty(string propertyName, Property<TProperty, TOwner> property)
        {
            Dictionary<string, Property<TProperty, TOwner>> properties;
            if (!registeredProperties.TryGetValue(typeof(TOwner), out properties))
            {
                properties = new Dictionary<string, Property<TProperty, TOwner>>();
                registeredProperties[typeof(TOwner)] = properties;
            }

            properties[propertyName] = property;
        }
    }
}