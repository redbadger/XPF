namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Text;
    using System.Text.RegularExpressions;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Presentation.Media;

    public class TextBlock : UIElement
    {
        public static readonly Property<Brush, TextBlock> BackgroundProperty =
            Property<Brush, TextBlock>.Register("Background", null);

        public static readonly Property<Brush, TextBlock> ForegroundProperty =
            Property<Brush, TextBlock>.Register("Foreground", null);

        public static readonly Property<Thickness, TextBlock> PaddingProperty =
            Property<Thickness, TextBlock>.Register(
                "Padding", new Thickness(), PropertyChangedCallbacks.InvalidateMeasure);

        public static readonly Property<string, TextBlock> TextProperty = Property<string, TextBlock>.Register(
            "Text", string.Empty, PropertyChangedCallbacks.InvalidateMeasure);

        public static readonly Property<TextWrapping, TextBlock> WrappingProperty =
            Property<TextWrapping, TextBlock>.Register(
                "Wrapping", TextWrapping.NoWrap, PropertyChangedCallbacks.InvalidateMeasure);

        private static readonly Regex whiteSpaceRegEx = new Regex(@"\s+", RegexOptions.Compiled);

        private readonly ISpriteFont spriteFont;

        private string formattedText;

        public TextBlock(ISpriteFont spriteFont)
        {
            this.spriteFont = spriteFont;
        }

        public Brush Background
        {
            get
            {
                return this.GetValue(BackgroundProperty);
            }

            set
            {
                this.SetValue(BackgroundProperty, value);
            }
        }

        public Brush Foreground
        {
            get
            {
                return this.GetValue(ForegroundProperty);
            }

            set
            {
                this.SetValue(ForegroundProperty, value);
            }
        }

        public Thickness Padding
        {
            get
            {
                return this.GetValue(PaddingProperty);
            }

            set
            {
                this.SetValue(PaddingProperty, value);
            }
        }

        public string Text
        {
            get
            {
                return this.GetValue(TextProperty);
            }

            set
            {
                this.SetValue(TextProperty, value);
            }
        }

        public TextWrapping Wrapping
        {
            get
            {
                return this.GetValue(WrappingProperty);
            }

            set
            {
                this.SetValue(WrappingProperty, value);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.formattedText = this.Text;
            Size measureString = this.spriteFont.MeasureString(this.formattedText);

            if (this.Wrapping == TextWrapping.Wrap && measureString.Width > availableSize.Width)
            {
                this.formattedText = WrapText(this.spriteFont, this.formattedText, availableSize.Width);
                measureString = this.spriteFont.MeasureString(this.formattedText);
            }

            return new Size(
                measureString.Width + this.Padding.Left + this.Padding.Right, 
                measureString.Height + this.Padding.Top + this.Padding.Bottom);
        }

        protected override void OnRender(IDrawingContext drawingContext)
        {
            if (this.Background != null)
            {
                drawingContext.DrawRectangle(new Rect(0, 0, this.ActualWidth, this.ActualHeight), this.Background);
            }

            drawingContext.DrawText(
                this.spriteFont, 
                this.formattedText, 
                new Vector(this.Padding.Left, this.Padding.Top), 
                this.Foreground ?? new SolidColorBrush(Colors.Black));
        }

        private static string WrapText(ISpriteFont font, string text, double maxLineWidth)
        {
            const string Space = " ";
            var stringBuilder = new StringBuilder();
            string[] words = whiteSpaceRegEx.Split(text);

            double lineWidth = 0;
            double spaceWidth = font.MeasureString(Space).Width;

            foreach (string word in words)
            {
                Size size = font.MeasureString(word);

                if (lineWidth + size.Width < maxLineWidth)
                {
                    stringBuilder.AppendFormat("{0}{1}", lineWidth == 0 ? string.Empty : Space, word);
                    lineWidth += size.Width + spaceWidth;
                }
                else
                {
                    stringBuilder.AppendFormat("\n{0}", word);
                    lineWidth = size.Width + spaceWidth;
                }
            }

            return stringBuilder.ToString();
        }
    }
}