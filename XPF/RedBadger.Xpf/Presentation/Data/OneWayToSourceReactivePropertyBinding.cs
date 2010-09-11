namespace RedBadger.Xpf.Presentation.Data
{
    internal class OneWayToSourceReactivePropertyBinding<TSource, TProperty> : OneWayToSourceBinding<TProperty>
        where TSource : ReactiveObject
    {
        private readonly ReactiveProperty<TProperty, TSource> reactiveProperty;

        public OneWayToSourceReactivePropertyBinding(ReactiveObject source, ReactiveProperty<TProperty, TSource> reactiveProperty)
            : base(source.GetObserver(reactiveProperty))
        {
        }

        public OneWayToSourceReactivePropertyBinding(ReactiveProperty<TProperty, TSource> reactiveProperty)
            : base(BindingResolutionMode.Deferred)
        {
            this.reactiveProperty = reactiveProperty;
        }

        public override void Resolve(object dataContext)
        {
            var reactiveObject = dataContext as ReactiveObject;
            if (reactiveObject != null)
            {
                this.SetObserver(reactiveObject.GetObserver(this.reactiveProperty));
            }
        }
    }
}