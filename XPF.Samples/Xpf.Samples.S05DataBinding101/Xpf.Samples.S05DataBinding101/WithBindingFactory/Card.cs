namespace Xpf.Samples.S05DataBinding101.WithBindingFactory
{
    using System;

    using RedBadger.Xpf.Data;
    using RedBadger.Xpf.Media.Imaging;

    public class Card : INotifyPropertyChanged
    {
        private readonly TextureImage faceDownImage;

        private readonly TextureImage faceUpImage;

        private TextureImage cardImage;

        private bool? isCardFaceUp = false;

        public Card(TextureImage faceDownImage, TextureImage faceUpImage)
        {
            this.faceDownImage = faceDownImage;
            this.faceUpImage = faceUpImage;

            this.CardImage = this.faceDownImage;
        }

        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        public TextureImage CardImage
        {
            get
            {
                return this.cardImage;
            }

            set
            {
                if (this.cardImage != value)
                {
                    this.cardImage = value;
                    this.OnPropertyChanged("CardImage");
                }
            }
        }

        public bool? IsCardFaceUp
        {
            get
            {
                return this.isCardFaceUp;
            }

            set
            {
                if (this.isCardFaceUp != value)
                {
                    this.isCardFaceUp = value;
                    this.OnPropertyChanged("IsCardFaceUp");
                    this.OnIsCardFaceUpChanged(value);
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

        public void Reset()
        {
            this.IsCardFaceUp = false;
        }

        private void OnIsCardFaceUpChanged(bool? value)
        {
            if (value.HasValue)
            {
                this.CardImage = (bool)value ? this.faceUpImage : this.faceDownImage;
            }
        }
    }
}