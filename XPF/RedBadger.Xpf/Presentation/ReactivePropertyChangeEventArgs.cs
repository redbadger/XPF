namespace RedBadger.Xpf.Presentation
{
    public struct ReactivePropertyChangeEventArgs<T>
    {
        public readonly T NewValue;

        public readonly T OldValue;

        public readonly ReactiveProperty<T> Property;

        public ReactivePropertyChangeEventArgs(ReactiveProperty<T> property, T oldValue, T newValue)
        {
            this.Property = property;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }
}