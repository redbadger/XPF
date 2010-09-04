namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Text;
    using System.Text.RegularExpressions;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Presentation.Media;

    public class TextBlock : UIElement
    {
        public static readonly IDependencyProperty BackgroundProperty =
            DependencyProperty<Brush, TextBlock>.Register("Background", new PropertyMetadata(null));

        public static readonly IDependencyProperty ForegroundProperty =
            DependencyProperty<Brush, TextBlock>.Register("Foreground", new PropertyMetadata(null));

        public static readonly IDependencyProperty PaddingProperty =
            DependencyProperty<Thickness, TextBlock>.Register(
                "Padding", 
                new PropertyMetadata(new Thickness(), UIElementPropertyChangedCallbacks.InvalidateMeasureIfThicknessChanged));

        public static readonly IDependencyProperty TextProperty = DependencyProperty<string, TextBlock>.Register(
            "Text", new PropertyMetadata(string.Empty, TextPropertyChangedCallback));

        public static readonly IDependencyProperty WrappingProperty =
            DependencyProperty<TextWrapping, TextBlock>.Register(
                "Wrapping", new PropertyMetadata(TextWrapping.NoWrap, TextWrappingPropertyChangedCallback));

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
                return this.GetValue<Brush>(BackgroundProperty);
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
                return this.GetValue<Brush>(ForegroundProperty);
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
                return this.GetValue<Thickness>(PaddingProperty);
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
                return this.GetValue<string>(TextProperty);
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
                return this.GetValue<TextWrapping>(WrappingProperty);
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

        private static void TextPropertyChangedCallback(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var newValue = (string)args.NewValue;
            var oldValue = (string)args.OldValue;

            if (newValue != oldValue)
            {
                var uiElement = dependencyObject as UIElement;
                if (uiElement != null)
                {
                    uiElement.InvalidateMeasure();
                }
            }
        }

        private static void TextWrappingPropertyChangedCallback(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var newValue = (TextWrapping)args.NewValue;
            var oldValue = (TextWrapping)args.OldValue;

            if (newValue != oldValue)
            {
                var uiElement = dependencyObject as UIElement;
                if (uiElement != null)
                {
                    uiElement.InvalidateMeasure();
                }
            }
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