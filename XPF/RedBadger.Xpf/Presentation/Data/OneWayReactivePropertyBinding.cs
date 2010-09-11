namespace RedBadger.Xpf.Presentation.Data
{
    internal class OneWayReactivePropertyBinding<TSource, TProperty> : OneWayBinding<TProperty>
        where TSource : DependencyObject
    {
        private readonly ReactiveProperty<TProperty, TSource> reactiveProperty;

        public OneWayReactivePropertyBinding(
            DependencyObject source, ReactiveProperty<TProperty, TSource> reactiveProperty)
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
            var dependencyObject = dataContext as DependencyObject;
            if (dependencyObject != null)
            {
                this.SubscribeObserver(dependencyObject.GetObservable(this.reactiveProperty));
            }
        }
    }
}