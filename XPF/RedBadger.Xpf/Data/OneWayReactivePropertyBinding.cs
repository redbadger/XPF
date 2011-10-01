namespace RedBadger.Xpf.Data
{
    using System;
    using System.Reactive.Linq;

    internal class OneWayReactivePropertyBinding<T, TSource> : OneWayBinding<T>
        where TSource : class, IReactiveObject
    {
        private readonly ReactiveProperty<T> deferredProperty;

        private ReactiveObject deferredSource;

        public OneWayReactivePropertyBinding(IReactiveObject source, ReactiveProperty<T> reactiveProperty)
            : base(BindingResolutionMode.Immediate)
        {
            this.SourceObservable = source.GetObservable<T, TSource>(reactiveProperty);
        }

        public OneWayReactivePropertyBinding(ReactiveProperty<T> reactiveProperty)
            : base(BindingResolutionMode.Deferred)
        {
            this.deferredProperty = reactiveProperty;
            this.SourceObservable = Observable.Defer(this.GetDeferredObservable);
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

        private IObservable<T> GetDeferredObservable()
        {
            return this.deferredSource.GetObservable<T, TSource>(this.deferredProperty);
        }
    }
}