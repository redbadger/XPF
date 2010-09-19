namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;

    using RedBadger.Xpf.Internal;

    /// <summary>
    ///     Describes a Reactive Property in XPF's Reactive Property System
    ///     Reactive Properties can participate in data binding and integrate tightly with the Reactive Extensions
    /// </summary>
    /// <typeparam name = "T">The <see cref = "Type">Type</see> of the Property</typeparam>
    public class ReactiveProperty<T> : IReactiveProperty
    {
        private static readonly PropertyStore<T> registeredProperties = new PropertyStore<T>();

        private readonly Action<object, ReactivePropertyChangeEventArgs<T>> changedCallback;

        private readonly T defaultValue;

        private readonly string name;

        private readonly Type type;

        private ReactiveProperty(
            string name, Type type, T defaultValue, Action<object, ReactivePropertyChangeEventArgs<T>> changedCallback)
        {
            this.name = name;
            this.type = type;
            this.defaultValue = defaultValue;
            this.changedCallback = changedCallback;
        }

        /// <summary>
        ///     A Call-back that is called whenever the value of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see> changes
        /// </summary>
        public Action<object, ReactivePropertyChangeEventArgs<T>> ChangedCallback
        {
            get
            {
                return this.changedCallback;
            }
        }

        /// <summary>
        ///     The default value of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see>
        /// </summary>
        public T DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }

        /// <summary>
        ///     The Name of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see>
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        ///     The Type of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see>
        /// </summary>
        public Type Type
        {
            get
            {
                return this.type;
            }
        }

        /// <summary>
        ///     Registers a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> with the given <see cref = "ReactiveProperty{T}.Name">Name</see>.
        /// </summary>
        /// <param name = "propertyName">The name of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see>.</param>
        /// <param name = "ownerType">The <see cref = "Type">Type</see> of the owner of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <returns>The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> that has been registered.</returns>
        public static ReactiveProperty<T> Register(string propertyName, Type ownerType)
        {
            return Register(propertyName, ownerType, default(T));
        }

        /// <summary>
        ///     Registers a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> with the given
        ///     <see cref = "Name">Name</see> and default value
        /// </summary>
        /// <param name = "propertyName">The name of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <param name = "ownerType">The <see cref = "Type">Type</see> of the owner of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <param name = "defaultValue">A default value for the <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <returns>The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> that has been registered</returns>
        public static ReactiveProperty<T> Register(string propertyName, Type ownerType, T defaultValue)
        {
            return Register(propertyName, ownerType, defaultValue, null);
        }

        /// <summary>
        ///     Registers a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> with the given
        ///     <see cref = "Name">Name</see> 
        ///     and <see cref = "ChangedCallback">ChangedCallback</see>
        /// </summary>
        /// <param name = "propertyName">The name of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <param name = "ownerType">The <see cref = "Type">Type</see> of the owner of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <param name = "changedCallback">A method to call when the value of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see> changes.</param>
        /// <returns>The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> that has been registered</returns>
        public static ReactiveProperty<T> Register(
            string propertyName, Type ownerType, Action<object, ReactivePropertyChangeEventArgs<T>> changedCallback)
        {
            return Register(propertyName, ownerType, default(T), changedCallback);
        }

        /// <summary>
        ///     Registers a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> with the given
        ///     <see cref = "Name">Name</see>, 
        ///     <see cref = "DefaultValue">DefaultValue</see>
        ///     and <see cref = "ChangedCallback">ChangedCallback</see>
        /// </summary>
        /// <param name = "propertyName">The name of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <param name = "ownerType">The <see cref = "Type">Type</see> of the owner of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <param name = "defaultValue">A default value for the <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <param name = "changedCallback">A method to call when the value of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see> changes.</param>
        /// <returns>The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> that has been registered</returns>
        public static ReactiveProperty<T> Register(
            string propertyName, 
            Type ownerType, 
            T defaultValue, 
            Action<object, ReactivePropertyChangeEventArgs<T>> changedCallback)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("propertyName cannot be null or an empty string");
            }

            var property = new ReactiveProperty<T>(propertyName, ownerType, defaultValue, changedCallback);

            StoreRegisteredProperty(propertyName, ownerType, property);
            return property;
        }

        private static void StoreRegisteredProperty(string propertyName, Type ownerType, ReactiveProperty<T> property)
        {
            Dictionary<string, ReactiveProperty<T>> properties;
            if (!registeredProperties.TryGetValue(ownerType, out properties))
            {
                properties = new Dictionary<string, ReactiveProperty<T>>();
                registeredProperties[ownerType] = properties;
            }

            properties[propertyName] = property;
        }
    }
}