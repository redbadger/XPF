namespace RedBadger.Xpf.Presentation
{
    using System;

    public class PropertyMetadata
    {
        private readonly Action<DependencyObject, DependencyPropertyChangedEventArgs> propertyChangedCallback;

        private readonly object defaultValue;

        public PropertyMetadata(object defaultValue)
            : this(defaultValue, null)
        {
        }

        public PropertyMetadata(
            object defaultValue, Action<DependencyObject, DependencyPropertyChangedEventArgs> propertyChangedCallback)
        {
            this.defaultValue = defaultValue;
            this.propertyChangedCallback = propertyChangedCallback;
        }

        public Action<DependencyObject, DependencyPropertyChangedEventArgs> PropertyChangedCallback
        {
            get
            {
                return this.propertyChangedCallback;
            }
        }

        public object DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }
    }
}