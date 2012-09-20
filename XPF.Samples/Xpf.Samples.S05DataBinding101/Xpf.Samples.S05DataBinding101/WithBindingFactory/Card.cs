#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

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
