namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;

    using RedBadger.Xpf.Internal;

    public class Property<TProperty, TOwner> : IProperty
        where TOwner : class
    {
        public static readonly object UnsetValue = new object();

        private static readonly PropertyStore<TProperty, TOwner> registeredProperties =
            new PropertyStore<TProperty, TOwner>();

        private readonly Action<TOwner, PropertyChangedEventArgs<TProperty, TOwner>> changedCallback;

        private readonly TProperty defaultValue;

        private readonly string name;

        private Property(
            string name, 
            TProperty defaultValue, 
            Action<TOwner, PropertyChangedEventArgs<TProperty, TOwner>> changedCallback)
        {
            this.name = name;
            this.defaultValue = defaultValue;
            this.changedCallback = changedCallback;
        }

        public Action<TOwner, PropertyChangedEventArgs<TProperty, TOwner>> ChangedCallback
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

        public static Property<TProperty, TOwner> Register(string name, TProperty defaultValue)
        {
            return Register(name, defaultValue, null);
        }

        public static Property<TProperty, TOwner> Register(
            string name, 
            TProperty defaultValue, 
            Action<TOwner, PropertyChangedEventArgs<TProperty, TOwner>> changedCallback)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name cannot be null or an empty string");
            }

            var property = new Property<TProperty, TOwner>(name, defaultValue, changedCallback);

            StoreRegisteredProperty(name, property);
            return property;
        }

        internal static void StoreRegisteredProperty(string name, Property<TProperty, TOwner> property)
        {
            Dictionary<string, Property<TProperty, TOwner>> properties;
            if (!registeredProperties.TryGetValue(typeof(TOwner), out properties))
            {
                properties = new Dictionary<string, Property<TProperty, TOwner>>();
                registeredProperties[typeof(TOwner)] = properties;
            }

            properties[name] = property;
        }
    }
}