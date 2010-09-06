namespace RedBadger.Xpf.Presentation.Data
{
    using System;

    public interface INotifyPropertyChanged
    {
        event EventHandler<PropertyChangedEventArgs> PropertyChanged;
    }
}