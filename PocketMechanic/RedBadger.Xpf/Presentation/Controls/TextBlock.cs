namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;

    using Size = RedBadger.Xpf.Presentation.Size;

#if WINDOWS_PHONE
    using UIElement = RedBadger.Xpf.Presentation.UIElement;
#endif

    public class TextBlock : UIElement
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(TextBlock), new PropertyMetadata(string.Empty));

        private readonly ISpriteFont spriteFont;

        public TextBlock(ISpriteFont spriteFont)
        {
            this.spriteFont = spriteFont;
        }

        public string Text
        {
            get
            {
                return (string)this.GetValue(TextProperty);
            }

            set
            {
                this.SetValue(TextProperty, value);
            }
        }

        public override void Render(ISpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.spriteFont, this.Text, this.VisualOffset, Color.White);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return this.DesiredSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Vector2 measureString = this.spriteFont.MeasureString(this.Text);
            return new Size(measureString.X, measureString.Y);
        }
    }
}