namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
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

        private Color foreground = Color.Black;

        public TextBlock(ISpriteFont spriteFont)
        {
            this.spriteFont = spriteFont;
        }

        public Color Foreground
        {
            get
            {
                return this.foreground;
            }

            set
            {
                this.foreground = value;
            }
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

        public Thickness Padding { get; set; }

        public override void Render(ISpriteBatch spriteBatch)
        {
            Vector2 drawPosition = this.VisualOffset + new Vector2(this.Padding.Left, this.Padding.Top);
            spriteBatch.DrawString(this.spriteFont, this.Text, drawPosition, this.foreground);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return this.DesiredSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Vector2 measureString = this.spriteFont.MeasureString(this.Text);

            return new Size(measureString.X + this.Padding.Left + this.Padding.Right, measureString.Y + this.Padding.Top + this.Padding.Bottom);
        }
    }
}