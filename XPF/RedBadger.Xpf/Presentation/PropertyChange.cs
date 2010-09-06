namespace RedBadger.Xpf.Presentation
{
    public struct PropertyChange<TProperty, TOwner>
        where TOwner : class
    {
        public readonly TProperty NewValue;

        public readonly TProperty OldValue;

        public readonly TOwner Owner;

        public readonly Property<TProperty, TOwner> Property;

        public PropertyChange(
            TOwner owner, Property<TProperty, TOwner> property, TProperty oldValue, TProperty newValue)
        {
            this.Owner = owner;
            this.Property = property;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }
}