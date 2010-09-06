namespace RedBadger.Xpf.Presentation
{
    public interface IDependencyObject
    {
        TProperty GetValue<TProperty, TOwner>(Property<TProperty, TOwner> property) where TOwner : class;

        void SetValue<TProperty, TOwner>(Property<TProperty, TOwner> property, TProperty newValue) where TOwner : class;
    }
}