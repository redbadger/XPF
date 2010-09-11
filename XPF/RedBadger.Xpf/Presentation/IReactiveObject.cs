namespace RedBadger.Xpf.Presentation
{
    public interface IReactiveObject
    {
        void ClearValue(IReactiveProperty property);

        TProperty GetValue<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property) where TOwner : class;

        void SetValue<TProperty, TOwner>(ReactiveProperty<TProperty, TOwner> property, TProperty newValue)
            where TOwner : class;
    }
}