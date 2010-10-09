namespace RedBadger.Xpf.Data
{
    using System;
    using System.Globalization;
    using System.Linq;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    internal class OneWayToSourceReactivePropertyBinding<TTargetProp, TSourceProp, TSource> :
        OneWayToSourceBinding<TSourceProp>, 
        IOneWayToSourceBinding<TTargetProp>
        where TSource : class, IReactiveObject
    {
        private readonly ReactiveProperty<TSourceProp> reactiveProperty;

        private TTargetProp initialValue;

        public OneWayToSourceReactivePropertyBinding(
            IReactiveObject source, ReactiveProperty<TSourceProp> reactiveProperty)
            : base(source.GetObserver<TSourceProp, TSource>(reactiveProperty))
        {
        }

        public OneWayToSourceReactivePropertyBinding(ReactiveProperty<TSourceProp> reactiveProperty)
            : base(BindingResolutionMode.Deferred)
        {
            this.reactiveProperty = reactiveProperty;
        }

        public override void Resolve(object dataContext)
        {
            var reactiveObject = dataContext as ReactiveObject;
            if (reactiveObject != null)
            {
                this.TargetObserver = reactiveObject.GetObserver<TSourceProp, TSource>(this.reactiveProperty);
                this.OnNext(this.initialValue);
            }
        }

        public void OnNext(TTargetProp value)
        {
            if (this.TargetObserver != null)
            {
                this.TargetObserver.OnNext(Convert(value));
            }
        }

        public IDisposable Initialize(IObservable<TTargetProp> observable)
        {
            this.Subscription = observable.Subscribe(this);
            this.initialValue = observable.FirstOrDefault();
            return this;
        }

        private static TSourceProp Convert(TTargetProp value)
        {
            return (TSourceProp)System.Convert.ChangeType(value, typeof(TSourceProp), CultureInfo.InvariantCulture);
        }
    }
}