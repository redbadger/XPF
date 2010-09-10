namespace RedBadger.Xpf.Presentation.Data
{
    internal class OneWayToSourceReactivePropertyBinding<TSource, TProperty> : OneWayToSourceBinding<TProperty>
        where TSource : DependencyObject
    {
        private readonly ReactiveProperty<TProperty, TSource> reactiveProperty;

        public OneWayToSourceReactivePropertyBinding(DependencyObject source, ReactiveProperty<TProperty, TSource> reactiveProperty)
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
            var dependencyObject = dataContext as DependencyObject;
            if (dependencyObject != null)
            {
                this.SetObserver(dependencyObject.GetObserver(this.reactiveProperty));
            }
        }
    }
}