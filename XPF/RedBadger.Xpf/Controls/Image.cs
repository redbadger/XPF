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

namespace RedBadger.Xpf.Controls
{
    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Internal.Controls;
    using RedBadger.Xpf.Media;

    public class Image : UIElement
    {
        public static readonly ReactiveProperty<ImageSource> SourceProperty =
            ReactiveProperty<ImageSource>.Register(
                "Source", typeof(Image), null, ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<StretchDirection> StretchDirectionProperty =
            ReactiveProperty<StretchDirection>.Register(
                "StretchDirection", 
                typeof(Image), 
                StretchDirection.Both, 
                ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<Stretch> StretchProperty = ReactiveProperty<Stretch>.Register(
            "Stretch", typeof(Image), Stretch.Uniform, ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public ImageSource Source
        {
            get
            {
                return this.GetValue(SourceProperty);
            }

            set
            {
                this.SetValue(SourceProperty, value);
            }
        }

        public Stretch Stretch
        {
            get
            {
                return this.GetValue(StretchProperty);
            }

            set
            {
                this.SetValue(StretchProperty, value);
            }
        }

        public StretchDirection StretchDirection
        {
            get
            {
                return this.GetValue(StretchDirectionProperty);
            }

            set
            {
                this.SetValue(StretchDirectionProperty, value);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return this.GetScaledImageSize(finalSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return this.GetScaledImageSize(availableSize);
        }

        protected override void OnRender(IDrawingContext drawingContext)
        {
            if (this.Source != null)
            {
                drawingContext.DrawImage(this.Source, new Rect(new Point(), this.RenderSize));
            }
        }

        private Size GetScaledImageSize(Size givenSize)
        {
            ImageSource source = this.Source;
            if (source == null)
            {
                return new Size();
            }

            Size contentSize = source.Size;
            Vector scale = Viewbox.ComputeScaleFactor(givenSize, contentSize, this.Stretch, this.StretchDirection);
            return new Size(contentSize.Width * scale.X, contentSize.Height * scale.Y);
        }
    }
}
