namespace RedBadger.Xpf.Presentation.Data
{
    using System;

    /// <summary>
    ///     Classes that implement this interface can participate in data binding with XPF's Reactive Property System
    ///     Implementors should raise the <see cref = "PropertyChanged">PropertyChanged</see> event when a property's value is changed
    /// </summary>
    public interface INotifyPropertyChanged
    {
        /// <summary>
        ///     This event is raised then the value of a property is changed
        /// </summary>
        event EventHandler<PropertyChangedEventArgs> PropertyChanged;
    }
}