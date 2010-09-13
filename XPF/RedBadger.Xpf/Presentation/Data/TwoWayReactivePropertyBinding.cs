namespace RedBadger.Xpf.Presentation.Data
{
    internal class TwoWayReactivePropertyBinding<TSource, TProperty> : TwoWayBinding<TProperty>
        where TSource : class, IReactiveObject
    {
        public TwoWayReactivePropertyBinding(ReactiveProperty<TProperty, TSource> reactiveProperty)
            : base(
                new OneWayReactivePropertyBinding<TSource, TProperty>(reactiveProperty), 
                new OneWayToSourceReactivePropertyBinding<TSource, TProperty>(reactiveProperty), 
                BindingResolutionMode.Deferred)
        {
        }

        public TwoWayReactivePropertyBinding(
            IReactiveObject source, ReactiveProperty<TProperty, TSource> reactiveProperty)
            : base(
                new OneWayReactivePropertyBinding<TSource, TProperty>(source, reactiveProperty), 
                new OneWayToSourceReactivePropertyBinding<TSource, TProperty>(source, reactiveProperty), 
                BindingResolutionMode.Immediate)
        {
        }
    }
}