namespace RedBadger.Xpf.Presentation.Controls
{
    using RedBadger.Xpf.Internal.Controls;
    using RedBadger.Xpf.Presentation.Media;

    public class Image : UIElement
    {
        public static readonly IDependencyProperty SourceProperty = DependencyProperty<ImageSource, Image>.Register(
            "Source", new PropertyMetadata(null, SourcePropertyChangedCallback));

        public static readonly IDependencyProperty StretchDirectionProperty =
            DependencyProperty<StretchDirection, Image>.Register(
                "StretchDirection", 
                new PropertyMetadata(StretchDirection.Both, StretchDirectionPropertyChangedCallback));

        public static readonly IDependencyProperty StretchProperty = DependencyProperty<Stretch, Image>.Register(
            "Stretch", 
            new PropertyMetadata(Stretch.Uniform, StretchPropertyChangedCallback));

        public ImageSource Source
        {
            get
            {
                return this.GetValue<ImageSource>(SourceProperty);
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
                return this.GetValue<Stretch>(StretchProperty);
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
                return this.GetValue<StretchDirection>(StretchDirectionProperty);
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
            drawingContext.DrawImage(this.Source, new Rect(new Point(), this.RenderSize));
        }

        private static void SourcePropertyChangedCallback(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var newValue = (ImageSource)args.NewValue;
            var oldValue = (ImageSource)args.OldValue;

            if (newValue != oldValue)
            {
                var uiElement = dependencyObject as UIElement;
                if (uiElement != null)
                {
                    uiElement.InvalidateMeasure();
                }
            }
        }

        private static void StretchDirectionPropertyChangedCallback(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var newValue = (StretchDirection)args.NewValue;
            var oldValue = (StretchDirection)args.OldValue;

            if (newValue != oldValue)
            {
                var uiElement = dependencyObject as UIElement;
                if (uiElement != null)
                {
                    uiElement.InvalidateMeasure();
                }
            }
        }

        private static void StretchPropertyChangedCallback(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var newValue = (Stretch)args.NewValue;
            var oldValue = (Stretch)args.OldValue;

            if (newValue != oldValue)
            {
                var uiElement = dependencyObject as UIElement;
                if (uiElement != null)
                {
                    uiElement.InvalidateMeasure();
                }
            }
        }

        private Size GetScaledImageSize(Size givenSize)
        {
            ImageSource source = this.Source;
            if (source == null)
            {
                return Size.Empty;
            }

            Size contentSize = source.Size;
            Vector scale = Viewbox.ComputeScaleFactor(givenSize, contentSize, this.Stretch, this.StretchDirection);
            return new Size(contentSize.Width * scale.X, contentSize.Height * scale.Y);
        }
    }
}