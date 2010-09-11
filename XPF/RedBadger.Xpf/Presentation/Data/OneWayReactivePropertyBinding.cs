namespace RedBadger.Xpf.Presentation.Data
{
    internal class OneWayReactivePropertyBinding<TSource, TProperty> : OneWayBinding<TProperty>
        where TSource : ReactiveObject
    {
        private readonly ReactiveProperty<TProperty, TSource> reactiveProperty;

        public OneWayReactivePropertyBinding(
            ReactiveObject source, ReactiveProperty<TProperty, TSource> reactiveProperty)
            : base(source.GetObservable(reactiveProperty))
        {
        }

        public OneWayReactivePropertyBinding(ReactiveProperty<TProperty, TSource> reactiveProperty)
            : base(BindingResolutionMode.Deferred)
        {
            this.reactiveProperty = reactiveProperty;
        }

        public override void Resolve(object dataContext)
        {
            var reactiveObject = dataContext as ReactiveObject;
            if (reactiveObject != null)
            {
                this.SubscribeObserver(reactiveObject.GetObservable(this.reactiveProperty));
            }
        }
    }
}