namespace RedBadger.Xpf.Presentation
{
    using System;

    internal class PropertyChangedEventArgs : EventArgs
    {
        public PropertyChangedEventArgs(object oldValue, object newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public object NewValue { get; private set; }

        public object OldValue { get; private set; }
    }
}