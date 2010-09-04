namespace RedBadger.Xpf.Presentation
{
    public class DependencyPropertyChangedEventArgs
    {
        private readonly DependencyProperty dependencyProperty;

        private readonly object newValue;

        private readonly object oldValue;

        public DependencyPropertyChangedEventArgs(
            DependencyProperty dependencyProperty, object oldValue, object newValue)
        {
            this.dependencyProperty = dependencyProperty;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public DependencyProperty DependencyProperty
        {
            get
            {
                return this.dependencyProperty;
            }
        }

        public object NewValue
        {
            get
            {
                return this.newValue;
            }
        }

        public object OldValue
        {
            get
            {
                return this.oldValue;
            }
        }
    }
}