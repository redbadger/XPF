namespace XpfSamples.S02ApplicationBar
{
    using System;

    using RedBadger.Xpf.Presentation.Data;
    using RedBadger.Xpf.Presentation.Media;

    public class ApplicationBarIconButton : INotifyPropertyChanged
    {
        public ApplicationBarIconButton(SolidColorBrush color)
        {
            this.Color = color;
        }

        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        public SolidColorBrush Color { get; set; }

        public void OnPropertyChanged(string propertyName)
        {
            EventHandler<PropertyChangedEventArgs> handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}