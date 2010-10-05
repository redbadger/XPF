namespace RedBadger.Wpug.Basketball.Domain
{
    using System;

    using RedBadger.Xpf.Data;

    public class Team : INotifyPropertyChanged
    {
        private int score;

        public Team(string name)
        {
            this.Name = name;
        }

        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        public string Name { get; set; }

        public int Score
        {
            get
            {
                return this.score;
            }

            set
            {
                if (this.score != value)
                {
                    this.score = value;
                    this.OnPropertyChanged("Score");
                }
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            EventHandler<PropertyChangedEventArgs> handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void IncrementScore(int points)
        {
            this.Score += points;
        }
    }
}