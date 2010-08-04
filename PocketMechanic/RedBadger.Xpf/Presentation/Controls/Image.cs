namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation.Media;

    using Size = RedBadger.Xpf.Presentation.Size;

    public class Image : UIElement
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(ImageSource), typeof(Image), new PropertyMetadata(null));

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

        public override void Render(ISpriteBatch spriteBatch)
        {
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            ImageSource source = this.Source;
            if (source == null)
            {
                return Size.Empty;
            }

            return source.Size;
        }
    }
}