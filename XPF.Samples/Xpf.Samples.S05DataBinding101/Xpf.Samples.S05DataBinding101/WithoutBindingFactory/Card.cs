namespace Xpf.Samples.S05DataBinding101.WithoutBindingFactory
{
    using System;

    using Microsoft.Phone.Reactive;

    using RedBadger.Xpf.Media;
    using RedBadger.Xpf.Media.Imaging;

    public class Card
    {
        private readonly TextureImage faceDownImage;

        private readonly TextureImage faceUpImage;

        private readonly ISubject<ImageSource> cardImage = new Subject<ImageSource>();

        private readonly ISubject<bool?> isCardFaceUp = new Subject<bool?>();

        public Card(TextureImage faceDownImage, TextureImage faceUpImage)
        {
            this.faceDownImage = faceDownImage;
            this.faceUpImage = faceUpImage;

            this.isCardFaceUp.Subscribe(this.OnIsCardFaceUpChanged);

            this.cardImage.OnNext(this.faceDownImage);
            this.isCardFaceUp.OnNext(false);
        }

        public IObservable<ImageSource> CardImage
        {
            get
            {
                return this.cardImage.AsObservable();
            }
        }

        public ISubject<bool?> IsCardFaceUp
        {
            get
            {
                return this.isCardFaceUp;
            }
        }

        public void Reset()
        {
            this.isCardFaceUp.OnNext(false);
        }

        private void OnIsCardFaceUpChanged(bool? value)
        {
            if (value.HasValue)
            {
                this.cardImage.OnNext((bool)value ? this.faceUpImage : this.faceDownImage);
            }
        }
    }
}