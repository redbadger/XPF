#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

namespace RedBadger.Xpf
{
    using System;
    using System.Collections.Generic;

    using RedBadger.Xpf.Internal;

    /// <summary>
    ///     Describes a Reactive Property in XPF's Reactive Property System
    ///     Reactive Properties can participate in data binding and integrate tightly with the Reactive Extensions
    /// </summary>
    /// <typeparam name = "T">The <see cref = "OwnerType">Type</see> of the Property</typeparam>
    public class ReactiveProperty<T> : IReactiveProperty
    {
        private static readonly PropertyStore<T> RegisteredProperties = new PropertyStore<T>();

        private readonly Action<IReactiveObject, ReactivePropertyChangeEventArgs<T>> changedCallback;

        private readonly T defaultValue;

        private readonly string name;

        private readonly Type ownerType;

        private ReactiveProperty(
            string name, 
            Type ownerType, 
            T defaultValue, 
            Action<IReactiveObject, ReactivePropertyChangeEventArgs<T>> changedCallback)
        {
            this.name = name;
            this.ownerType = ownerType;
            this.defaultValue = defaultValue;
            this.changedCallback = changedCallback;
        }

        /// <summary>
        ///     A Call-back that is called whenever the value of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see> changes
        /// </summary>
        public Action<IReactiveObject, ReactivePropertyChangeEventArgs<T>> ChangedCallback
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
        ///     The Type of the owner of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see>
        /// </summary>
        public Type OwnerType
        {
            get
            {
                return this.ownerType;
            }
        }

        /// <summary>
        ///     Registers a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> with the given <see cref = "ReactiveProperty{T}.Name">Name</see>.
        /// </summary>
        /// <param name = "propertyName">The name of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see>.</param>
        /// <param name = "ownerType">The <see cref = "OwnerType">Type</see> of the owner of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
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
        /// <param name = "ownerType">The <see cref = "OwnerType">Type</see> of the owner of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
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
        /// <param name = "ownerType">The <see cref = "OwnerType">Type</see> of the owner of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <param name = "changedCallback">A method to call when the value of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see> changes.</param>
        /// <returns>The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> that has been registered</returns>
        public static ReactiveProperty<T> Register(
            string propertyName, 
            Type ownerType, 
            Action<IReactiveObject, ReactivePropertyChangeEventArgs<T>> changedCallback)
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
        /// <param name = "ownerType">The <see cref = "OwnerType">Type</see> of the owner of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <param name = "defaultValue">A default value for the <see cref = "ReactiveProperty{T}">ReactiveProperty</see></param>
        /// <param name = "changedCallback">A method to call when the value of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see> changes.</param>
        /// <returns>The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> that has been registered</returns>
        public static ReactiveProperty<T> Register(
            string propertyName, 
            Type ownerType, 
            T defaultValue, 
            Action<IReactiveObject, ReactivePropertyChangeEventArgs<T>> changedCallback)
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
            if (!RegisteredProperties.TryGetValue(ownerType, out properties))
            {
                properties = new Dictionary<string, ReactiveProperty<T>>();
                RegisteredProperties[ownerType] = properties;
            }

            properties[propertyName] = property;
        }
    }
}
