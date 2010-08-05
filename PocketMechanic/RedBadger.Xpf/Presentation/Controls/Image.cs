namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation.Media;

    using Size = RedBadger.Xpf.Presentation.Size;
    using UIElement = RedBadger.Xpf.Presentation.UIElement;

    public class Image : UIElement
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(ImageSource), typeof(Image), new PropertyMetadata(null, SourcePropertyChangedCallback));

        public static readonly DependencyProperty StretchDirectionProperty =
            DependencyProperty.Register(
                "StretchDirection", 
                typeof(StretchDirection), 
                typeof(Image), 
                new PropertyMetadata(StretchDirection.Both, StretchDirectionPropertyChangedCallback));

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch", 
            typeof(Stretch), 
            typeof(Image), 
            new PropertyMetadata(Stretch.Uniform, StretchPropertyChangedCallback));

        public ImageSource Source
        {
            get
            {
                return (ImageSource)this.GetValue(SourceProperty);
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
                return (Stretch)this.GetValue(StretchProperty);
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
                return (StretchDirection)this.GetValue(StretchDirectionProperty);
            }

            set
            {
                this.SetValue(StretchDirectionProperty, value);
            }
        }

        public override void Render(ISpriteBatch spriteBatch)
        {
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return this.GetScaledImageSize(finalSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return this.GetScaledImageSize(availableSize);
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
            Vector2 scale = Viewbox.ComputeScaleFactor(givenSize, contentSize, this.Stretch, this.StretchDirection);
            return new Size(contentSize.Width * scale.X, contentSize.Height * scale.Y);
        }
    }
}