namespace RedBadger.Xpf.Presentation.Data
{
    using System;

    /// <summary>
    ///     EventArgs used with <see cref = "INotifyPropertyChanged">INotifyPropertyChanged</see> to indicate that a property's value has changed.
    /// </summary>
    public class PropertyChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Constructs a new instance of PropertyChangedEventArgs.
        /// </summary>
        /// <param name = "propertyName">The <see cref = "PropertyName">PropertyName</see> of the property whose value has changed</param>
        public PropertyChangedEventArgs(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        ///     The name of the property whose value has changed
        /// </summary>
        public string PropertyName { get; private set; }
    }
}