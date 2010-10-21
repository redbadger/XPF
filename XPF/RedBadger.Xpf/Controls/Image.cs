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