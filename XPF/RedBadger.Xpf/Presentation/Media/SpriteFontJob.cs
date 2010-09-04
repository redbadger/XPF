namespace RedBadger.Xpf.Presentation.Media
{
    using RedBadger.Xpf.Graphics;

    internal struct SpriteFontJob : ISpriteJob
    {
        private readonly Brush brush;

        private readonly Vector position;

        private readonly ISpriteFont spriteFont;

        private readonly string text;

        private Vector absoluteOffset;

        public SpriteFontJob(ISpriteFont spriteFont, string text, Vector position, Brush brush)
        {
            this.spriteFont = spriteFont;
            this.text = text;
            this.position = position;
            this.brush = brush;
            this.absoluteOffset = Vector.Zero;
        }

        public void Draw(ISpriteBatch spriteBatch)
        {
            var solidColorBrush = this.brush as SolidColorBrush;
            spriteBatch.DrawString(
                this.spriteFont, 
                this.text, 
                this.position + this.absoluteOffset, 
                solidColorBrush != null ? solidColorBrush.Color : Colors.Magenta);
        }

        public void SetAbsoluteOffset(Vector offset)
        {
            this.absoluteOffset = offset;
        }
    }
}