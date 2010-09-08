namespace RedBadger.Xpf.Presentation
{
    public struct ReactivePropertyChange<TProperty, TOwner>
        where TOwner : class
    {
        public readonly TProperty NewValue;

        public readonly TProperty OldValue;

        public readonly TOwner Owner;

        public readonly ReactiveProperty<TProperty, TOwner> Property;

        public ReactivePropertyChange(
            TOwner owner, ReactiveProperty<TProperty, TOwner> property, TProperty oldValue, TProperty newValue)
        {
            this.Owner = owner;
            this.Property = property;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }
}