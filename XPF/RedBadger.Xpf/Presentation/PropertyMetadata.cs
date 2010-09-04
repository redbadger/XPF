namespace RedBadger.Xpf.Presentation
{
    using System;

    public class PropertyMetadata
    {
        public PropertyMetadata(object defaultValue)
            : this(defaultValue, null)
        {
        }

        public PropertyMetadata(
            object defaultValue, Action<DependencyObject, DependencyPropertyChangedEventArgs> changedCallback)
        {
        }
    }
}