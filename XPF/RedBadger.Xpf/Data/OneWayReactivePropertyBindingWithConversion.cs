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
    using System.Globalization;
    
    using System.Reactive.Linq;

    internal class OneWayReactivePropertyBinding<TTargetProp, TSourceProp, TSource> : OneWayBinding<TTargetProp>
        where TSource : class, IReactiveObject
    {
        private readonly ReactiveProperty<TSourceProp> deferredProperty;

        private ReactiveObject deferredSource;

        public OneWayReactivePropertyBinding(IReactiveObject source, ReactiveProperty<TSourceProp> reactiveProperty)
            : base(BindingResolutionMode.Immediate)
        {
            this.SourceObservable = source.GetObservable<TSourceProp, TSource>(reactiveProperty).Select(Convert);
        }

        public OneWayReactivePropertyBinding(ReactiveProperty<TSourceProp> reactiveProperty)
            : base(BindingResolutionMode.Deferred)
        {
            this.deferredProperty = reactiveProperty;
            this.SourceObservable = Observable.Defer(this.GetDeferredObservable).Select(Convert);
        }

        public override void Resolve(object dataContext)
        {
            var reactiveObject = dataContext as ReactiveObject;
            if (reactiveObject != null)
            {
                this.deferredSource = reactiveObject;
                this.Subscription = this.SourceObservable.Subscribe(this.Observer);
            }
        }

        protected IObservable<TSourceProp> GetDeferredObservable()
        {
            return this.deferredSource.GetObservable<TSourceProp, TSource>(this.deferredProperty);
        }

        private static TTargetProp Convert(TSourceProp value)
        {
            return (TTargetProp)System.Convert.ChangeType(value, typeof(TTargetProp), CultureInfo.InvariantCulture);
        }
    }
}
