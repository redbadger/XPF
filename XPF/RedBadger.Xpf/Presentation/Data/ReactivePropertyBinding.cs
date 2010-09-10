namespace RedBadger.Xpf.Presentation.Data
{
    internal class ReactivePropertyBinding<TSource, TProperty> : Binding<TProperty>
        where TSource : DependencyObject
    {
        private readonly ReactiveProperty<TProperty, TSource> reactiveProperty;

        public ReactivePropertyBinding(DependencyObject source, ReactiveProperty<TProperty, TSource> reactiveProperty)
            : base(source.GetObservable(reactiveProperty))
        {
        }

        public ReactivePropertyBinding(ReactiveProperty<TProperty, TSource> reactiveProperty)
            : base(BindingResolutionMode.Deferred)
        {
            this.reactiveProperty = reactiveProperty;
        }

        public override void Resolve(object dataContext)
        {
            this.SubscribeToObserver();

            var dependencyObject = dataContext as DependencyObject;
            if (dependencyObject != null)
            {
                dependencyObject.GetObservable(this.reactiveProperty).Subscribe(this.Subject);
            }
        }
    }
}