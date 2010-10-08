namespace RedBadger.Xpf.Data
{
    using System.Globalization;
    using System.Linq;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    internal class OneWayReactivePropertyBinding<TTargetProp, TSourceProp, TSource> : OneWayBinding<TTargetProp>
        where TSource : class, IReactiveObject
    {
        private readonly ReactiveProperty<TSourceProp> reactiveProperty;

        public OneWayReactivePropertyBinding(IReactiveObject source, ReactiveProperty<TSourceProp> reactiveProperty)
            : base(source.GetObservable<TSourceProp, TSource>(reactiveProperty).Select(Convert))
        {
        }

        public OneWayReactivePropertyBinding(ReactiveProperty<TSourceProp> reactiveProperty)
            : base(BindingResolutionMode.Deferred)
        {
            this.reactiveProperty = reactiveProperty;
        }

        public override void Resolve(object dataContext)
        {
            var reactiveObject = dataContext as ReactiveObject;
            if (reactiveObject != null)
            {
                this.SubscribeObserver(
                    reactiveObject.GetObservable<TSourceProp, TSource>(this.reactiveProperty).Select(Convert));
            }
        }

        private static TTargetProp Convert(TSourceProp o)
        {
            return (TTargetProp)System.Convert.ChangeType(o, typeof(TTargetProp), CultureInfo.InvariantCulture);
        }
    }
}