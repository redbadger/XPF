namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;

    using RedBadger.Xpf.Internal;

    /// <summary>
    ///     Describes a Reactive Property in XPF's Reactive Property System
    ///     Reactive Properties can participate in data binding and integrate tightly with the Reactive Extensions
    /// </summary>
    /// <typeparam name = "TProperty">The <see cref = "Type">Type</see> of the Property</typeparam>
    /// <typeparam name = "TOwner">The <see cref = "Type">Type</see> of the owner of the property</typeparam>
    public class ReactiveProperty<TProperty, TOwner> : IReactiveProperty
        where TOwner : class
    {
        private static readonly PropertyStore<TProperty, TOwner> registeredProperties =
            new PropertyStore<TProperty, TOwner>();

        private readonly Action<TOwner, ReactivePropertyChangeEventArgs<TProperty, TOwner>> changedCallback;

        private readonly TProperty defaultValue;

        private readonly string name;

        private ReactiveProperty(
            string name, 
            TProperty defaultValue, 
            Action<TOwner, ReactivePropertyChangeEventArgs<TProperty, TOwner>> changedCallback)
        {
            this.name = name;
            this.defaultValue = defaultValue;
            this.changedCallback = changedCallback;
        }

        /// <summary>
        ///     A Call-back that is called whenever the value of the <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> changes
        /// </summary>
        public Action<TOwner, ReactivePropertyChangeEventArgs<TProperty, TOwner>> ChangedCallback
        {
            get
            {
                return this.changedCallback;
            }
        }

        /// <summary>
        ///     The default value of the <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see>
        /// </summary>
        public TProperty DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }

        /// <summary>
        ///     The Name of the <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see>
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        ///     Registers a <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> with the given <see cref = "ReactiveProperty{TProperty,TOwner}.Name">Name</see>.
        /// </summary>
        /// <param name = "propertyName">The name of the <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see>.</param>
        /// <returns>The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> that has been registered.</returns>
        public static ReactiveProperty<TProperty, TOwner> Register(string propertyName)
        {
            return Register(propertyName, default(TProperty));
        }

        /// <summary>
        ///     Registers a <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> with the given
        ///     <see cref = "Name">Name</see> and default value
        /// </summary>
        /// <param name = "propertyName">The name of the <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see></param>
        /// <param name = "defaultValue">A default value for the <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see></param>
        /// <returns>The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> that has been registered</returns>
        public static ReactiveProperty<TProperty, TOwner> Register(string propertyName, TProperty defaultValue)
        {
            return Register(propertyName, defaultValue, null);
        }

        /// <summary>
        ///     Registers a <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> with the given
        ///     <see cref = "Name">Name</see> 
        ///     and <see cref = "ChangedCallback">ChangedCallback</see>
        /// </summary>
        /// <param name = "propertyName">The name of the <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see></param>
        /// <param name = "changedCallback">A method to call when the value of the <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> changes.</param>
        /// <returns>The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> that has been registered</returns>
        public static ReactiveProperty<TProperty, TOwner> Register(
            string propertyName, Action<TOwner, ReactivePropertyChangeEventArgs<TProperty, TOwner>> changedCallback)
        {
            return Register(propertyName, default(TProperty), changedCallback);
        }

        /// <summary>
        ///     Registers a <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> with the given
        ///     <see cref = "Name">Name</see>, 
        ///     <see cref = "DefaultValue">DefaultValue</see>
        ///     and <see cref = "ChangedCallback">ChangedCallback</see>
        /// </summary>
        /// <param name = "propertyName">The name of the <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see></param>
        /// <param name = "defaultValue">A default value for the <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see></param>
        /// <param name = "changedCallback">A method to call when the value of the <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> changes.</param>
        /// <returns>The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> that has been registered</returns>
        public static ReactiveProperty<TProperty, TOwner> Register(
            string propertyName, 
            TProperty defaultValue, 
            Action<TOwner, ReactivePropertyChangeEventArgs<TProperty, TOwner>> changedCallback)
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