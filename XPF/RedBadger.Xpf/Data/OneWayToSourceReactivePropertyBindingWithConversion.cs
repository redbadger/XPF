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

    internal class OneWayToSourceReactivePropertyBinding<TTargetProp, TSourceProp, TSource> :
        OneWayToSourceBinding<TSourceProp>, 
        IOneWayToSourceBinding<TTargetProp>
        where TSource : class, IReactiveObject
    {
        private readonly ReactiveProperty<TSourceProp> reactiveProperty;

        private TTargetProp initialValue;

        public OneWayToSourceReactivePropertyBinding(
            IReactiveObject source, ReactiveProperty<TSourceProp> reactiveProperty)
            : base(source.GetObserver<TSourceProp, TSource>(reactiveProperty))
        {
        }

        public OneWayToSourceReactivePropertyBinding(ReactiveProperty<TSourceProp> reactiveProperty)
            : base(BindingResolutionMode.Deferred)
        {
            this.reactiveProperty = reactiveProperty;
        }

        public override void Resolve(object dataContext)
        {
            var reactiveObject = dataContext as ReactiveObject;
            if (reactiveObject != null)
            {
                this.TargetObserver = reactiveObject.GetObserver<TSourceProp, TSource>(this.reactiveProperty);
                this.OnNext(this.initialValue);
            }
        }

        public void OnNext(TTargetProp value)
        {
            if (this.TargetObserver != null)
            {
                this.TargetObserver.OnNext(Convert(value));
            }
        }

        public IDisposable Initialize(IObservable<TTargetProp> observable)
        {
            this.Subscription = observable.Subscribe(this);
            this.initialValue = observable.FirstOrDefault();
            return this;
        }

        private static TSourceProp Convert(TTargetProp value)
        {
            return (TSourceProp)System.Convert.ChangeType(value, typeof(TSourceProp), CultureInfo.InvariantCulture);
        }
    }
}
