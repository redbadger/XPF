namespace RedBadger.Xpf.Presentation
{
    public struct ReactivePropertyChangeEventArgs<TProperty, TOwner>
        where TOwner : class
    {
        public readonly TProperty NewValue;

        public readonly TProperty OldValue;

        public readonly ReactiveProperty<TProperty, TOwner> Property;

        public ReactivePropertyChangeEventArgs(ReactiveProperty<TProperty, TOwner> property, TProperty oldValue, TProperty newValue)
        {
            this.Property = property;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }
}