namespace RedBadger.Xpf.Data
{
    internal class TwoWayReactivePropertyBinding<T, TSource> : TwoWayBinding<T>
        where TSource : class, IReactiveObject
    {
        public TwoWayReactivePropertyBinding(ReactiveProperty<T> reactiveProperty)
            : base(
                new OneWayReactivePropertyBinding<T, TSource>(reactiveProperty), 
                new OneWayToSourceReactivePropertyBinding<T, TSource>(reactiveProperty), 
                BindingResolutionMode.Deferred)
        {
        }

        public TwoWayReactivePropertyBinding(IReactiveObject source, ReactiveProperty<T> reactiveProperty)
            : base(
                new OneWayReactivePropertyBinding<T, TSource>(source, reactiveProperty), 
                new OneWayToSourceReactivePropertyBinding<T, TSource>(source, reactiveProperty), 
                BindingResolutionMode.Immediate)
        {
        }
    }
}