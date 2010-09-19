namespace RedBadger.Xpf.Presentation.Data
{
    internal class OneWayReactivePropertyBinding<T, TSource> : OneWayBinding<T>
        where TSource : class, IReactiveObject
    {
        private readonly ReactiveProperty<T> reactiveProperty;

        public OneWayReactivePropertyBinding(
            IReactiveObject source, ReactiveProperty<T> reactiveProperty)
            : base(source.GetObservable<T, TSource>(reactiveProperty))
        {
        }

        public OneWayReactivePropertyBinding(ReactiveProperty<T> reactiveProperty)
            : base(BindingResolutionMode.Deferred)
        {
            this.reactiveProperty = reactiveProperty;
        }

        public override void Resolve(object dataContext)
        {
            var reactiveObject = dataContext as ReactiveObject;
            if (reactiveObject != null)
            {
                this.SubscribeObserver(reactiveObject.GetObservable<T, TSource>(this.reactiveProperty));
            }
        }
    }
}