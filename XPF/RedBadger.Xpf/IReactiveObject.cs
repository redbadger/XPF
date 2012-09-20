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

    /// <summary>
    ///     Represents an object that participates in the Reactive Property system.
    /// </summary>
    public interface IReactiveObject
    {
        /// <summary>
        ///     Clears the value of a <see cref = "IReactiveProperty">IReactiveProperty</see> on this instance of <see cref = "IReactiveObject">IReactiveObject</see>.
        /// </summary>
        /// <param name = "property">The <see cref = "IReactiveProperty">IReactiveProperty</see> to clear.</param>
        void ClearValue(IReactiveProperty property);

        /// <summary>
        ///     Gets an <see cref = "IObservable{T}">IObservable</see> for a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> for this instance of <see cref = "ReactiveObject">ReactiveObject</see>.
        /// </summary>
        /// <typeparam name = "T">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see>'s <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> to retrieve a value for.</param>
        /// <returns>An <see cref = "IObservable{T}">IObservable</see> around the <see cref = "ReactiveProperty{T}">ReactiveProperty</see>.</returns>
        IObservable<T> GetObservable<T, TOwner>(ReactiveProperty<T> property) where TOwner : class, IReactiveObject;

        /// <summary>
        ///     Gets an <see cref = "IObserver{T}">IObserver</see> for a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> for this instance of <see cref = "ReactiveObject">ReactiveObject</see>.
        /// </summary>
        /// <typeparam name = "T">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see>'s <see cref = "Type">Type</see></typeparam>
        /// <typeparam name = "TOwner">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see>'s owner <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> to retrieve a value for.</param>
        /// <returns>An <see cref = "IObserver{T}">IObserver</see> around the <see cref = "ReactiveProperty{T}">ReactiveProperty</see>.</returns>
        IObserver<T> GetObserver<T, TOwner>(ReactiveProperty<T> property) where TOwner : class, IReactiveObject;

        /// <summary>
        ///     Gets the current value of a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on this instance of <see cref = "IReactiveObject">IReactiveObject</see>.
        /// </summary>
        /// <typeparam name = "T">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see>'s <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> to retrieve a value for.</param>
        /// <returns>The current value.</returns>
        T GetValue<T>(ReactiveProperty<T> property);

        /// <summary>
        ///     Sets the value of a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on this instance of <see cref = "IReactiveObject">IReactiveObject</see>.
        /// </summary>
        /// <typeparam name = "T">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see>'s <see cref = "Type">Type</see></typeparam>
        /// <param name = "property">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> to set.</param>
        /// <param name = "newValue">The new value.</param>
        void SetValue<T>(ReactiveProperty<T> property, T newValue);
    }
}
