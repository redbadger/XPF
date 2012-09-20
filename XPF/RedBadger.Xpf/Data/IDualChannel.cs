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

namespace RedBadger.Xpf.Data
{
    using System;

    /// <summary>
    ///     Represents two channels (an <see cref = "IObservable{T}">IObservable</see> and an <see cref = "IObserver{T}">IObserver</see>)
    /// </summary>
    /// <typeparam name = "T">The <see cref = "Type">Type</see> of the data in the channels</typeparam>
    public interface IDualChannel<T>
    {
        /// <summary>
        ///     The <see cref = "IObservable{T}">IObservable</see> channel
        /// </summary>
        IObservable<T> Observable { get; }

        /// <summary>
        ///     The <see cref = "IObserver{T}">IObserver</see> channel
        /// </summary>
        IObserver<T> Observer { get; }
    }
}
