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

namespace Xpf.Samples.S05DataBinding101.WithoutBindingFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
