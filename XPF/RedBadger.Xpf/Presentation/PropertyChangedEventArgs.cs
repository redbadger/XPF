namespace RedBadger.Xpf.Presentation
{
    public class PropertyChangedEventArgs<TProperty, TOwner> where TOwner : class
    {
        private readonly Property<TProperty, TOwner> property;

        private readonly TProperty newValue;

        private readonly TProperty oldValue;

        public PropertyChangedEventArgs(
            Property<TProperty, TOwner> property, TProperty oldValue, TProperty newValue)
        {
            this.property = property;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public Property<TProperty, TOwner> Property
        {
            get
            {
                return this.property;
            }
        }

        public TProperty NewValue
        {
            get
            {
                return this.newValue;
            }
        }

        public TProperty OldValue
        {
            get
            {
                return this.oldValue;
            }
        }
    }
}