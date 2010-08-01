namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;

    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Media;

    using Size = RedBadger.Xpf.Presentation.Size;

#if WINDOWS_PHONE
    using UIElement = RedBadger.Xpf.Presentation.UIElement;
    using Thickness = RedBadger.Xpf.Presentation.Thickness;
#endif

    public class TextBlock : UIElement
    {
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            "Foreground", typeof(Brush), typeof(TextBlock), new PropertyMetadata(null));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(TextBlock), new PropertyMetadata(string.Empty));

        private static readonly Regex WhiteSpaceRegEx = new Regex(@"\s+", RegexOptions.Compiled);

        private readonly ISpriteFont spriteFont;

        private string formattedText;

        public TextBlock(ISpriteFont spriteFont)
        {
            this.spriteFont = spriteFont;
        }

        public Brush Foreground
        {
            get
            {
                return (Brush)this.GetValue(ForegroundProperty);
            }

            set
            {
                this.SetValue(ForegroundProperty, value);
            }
        }

        public Thickness Padding { get; set; }

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

        public TextWrapping Wrapping { get; set; }

        public override void Render(ISpriteBatch spriteBatch)
        {
            Vector2 drawPosition = this.VisualOffset + new Vector2(this.Padding.Left, this.Padding.Top);
            var brush = this.Foreground as SolidColorBrush;
            spriteBatch.DrawString(
                this.spriteFont, this.formattedText, drawPosition, brush != null ? brush.Color : Color.Black);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return this.DesiredSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.formattedText = this.Text;
            Vector2 measureString = this.spriteFont.MeasureString(this.formattedText);

            if (this.Wrapping == TextWrapping.Wrap && measureString.X > availableSize.Width)
            {
                this.formattedText = WrapText(this.spriteFont, this.formattedText, availableSize.Width);
                measureString = this.spriteFont.MeasureString(this.formattedText);
            }

            return new Size(
                measureString.X + this.Padding.Left + this.Padding.Right, 
                measureString.Y + this.Padding.Top + this.Padding.Bottom);
        }

        private static string WrapText(ISpriteFont font, string text, float maxLineWidth)
        {
            const string Space = " ";
            var stringBuilder = new StringBuilder();
            string[] words = WhiteSpaceRegEx.Split(text);

            float lineWidth = 0f;
            float spaceWidth = font.MeasureString(Space).X;

            foreach (string word in words)
            {
                Vector2 size = font.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    stringBuilder.AppendFormat("{0}{1}", lineWidth == 0 ? string.Empty : Space, word);
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    stringBuilder.AppendFormat("\n{0}", word);
                    lineWidth = size.X + spaceWidth;
                }
            }

            return stringBuilder.ToString();
        }
    }
}