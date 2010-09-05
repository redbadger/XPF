/*namespace RedBadger.PocketMechanic.Phone
{
    using System;
    using System.ComponentModel;

    using Microsoft.Phone.Reactive;

    public class Clock : INotifyPropertyChanged
    {
        private readonly IObservable<long> timer = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1));

        private string time;

        public Clock()
        {
            this.timer.ObserveOnDispatcher().Subscribe(
                Observer.Create<long>(
                    l =>
                        {
                            DateTime dateTime = DateTime.Now;
                            this.Time = dateTime.Second % 2 == 0
                                            ? dateTime.ToLongTimeString()
                                            : dateTime.ToShortTimeString();
                        }));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Time
        {
            get
            {
                return this.time;
            }

            set
            {
                this.time = value;
                this.InvokePropertyChanged(new PropertyChangedEventArgs<TProperty, TOwner>("Time"));
            }
        }

        public void InvokePropertyChanged(PropertyChangedEventArgs<TProperty, TOwner> e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}*/