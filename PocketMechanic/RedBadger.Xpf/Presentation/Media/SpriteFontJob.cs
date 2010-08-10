namespace RedBadger.Xpf.Presentation.Media
{
    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;

    internal struct SpriteFontJob : ISpriteJob
    {
        private readonly Brush brush;

        private readonly Vector2 position;

        private readonly ISpriteFont spriteFont;

        private readonly string text;

        private Vector2 absoluteOffset;

        public SpriteFontJob(ISpriteFont spriteFont, string text, Vector2 position, Brush brush)
        {
            this.spriteFont = spriteFont;
            this.text = text;
            this.position = position;
            this.brush = brush;
            this.absoluteOffset = Vector2.Zero;
        }

        public void SetAbsoluteOffset(Vector2 offset)
        {
            this.absoluteOffset = offset;
        }

        public void Draw(ISpriteBatch spriteBatch)
        {
            var solidColorBrush = this.brush as SolidColorBrush;
            spriteBatch.DrawString(
                this.spriteFont, this.text, this.position + this.absoluteOffset, solidColorBrush != null ? solidColorBrush.Color : Color.Black);
        }
    }
}