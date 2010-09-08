namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;

    using RedBadger.Xpf.Internal;

    public class ReactiveProperty<TProperty, TOwner> : IProperty
        where TOwner : class
    {
        private static readonly PropertyStore<TProperty, TOwner> registeredProperties =
            new PropertyStore<TProperty, TOwner>();

        private readonly Action<TOwner, ReactivePropertyChangeEventArgs<TProperty, TOwner>> changedCallback;

        private readonly TProperty defaultValue;

        private readonly string name;

        private ReactiveProperty(string name, TProperty defaultValue, Action<TOwner, ReactivePropertyChangeEventArgs<TProperty, TOwner>> changedCallback)
        {
            this.name = name;
            this.defaultValue = defaultValue;
            this.changedCallback = changedCallback;
        }

        public Action<TOwner, ReactivePropertyChangeEventArgs<TProperty, TOwner>> ChangedCallback
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

        public static ReactiveProperty<TProperty, TOwner> Register(string propertyName)
        {
            return Register(propertyName, default(TProperty));
        }

        public static ReactiveProperty<TProperty, TOwner> Register(string propertyName, TProperty defaultValue)
        {
            return Register(propertyName, defaultValue, null);
        }

        public static ReactiveProperty<TProperty, TOwner> Register(
            string propertyName, Action<TOwner, ReactivePropertyChangeEventArgs<TProperty, TOwner>> changedCallback)
        {
            return Register(propertyName, default(TProperty), changedCallback);
        }

        public static ReactiveProperty<TProperty, TOwner> Register(
            string propertyName, TProperty defaultValue, Action<TOwner, ReactivePropertyChangeEventArgs<TProperty, TOwner>> changedCallback)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("propertyName cannot be null or an empty string");
            }

            var property = new ReactiveProperty<TProperty, TOwner>(propertyName, defaultValue, changedCallback);

            StoreRegisteredProperty(propertyName, property);
            return property;
        }

        private static void StoreRegisteredProperty(string propertyName, ReactiveProperty<TProperty, TOwner> property)
        {
            Dictionary<string, ReactiveProperty<TProperty, TOwner>> properties;
            if (!registeredProperties.TryGetValue(typeof(TOwner), out properties))
            {
                properties = new Dictionary<string, ReactiveProperty<TProperty, TOwner>>();
                registeredProperties[typeof(TOwner)] = properties;
            }

            properties[propertyName] = property;
        }
    }
}