namespace RedBadger.Xpf.Data
{
    using System;
    using System.Globalization;
    using System.Linq;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

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