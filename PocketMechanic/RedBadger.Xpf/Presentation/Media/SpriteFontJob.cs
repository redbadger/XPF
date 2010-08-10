namespace RedBadger.Xpf.Presentation.Media
{
    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;

    internal struct SpriteFontJob : ISpriteJob
    {
        private readonly Color color;

        private readonly ISpriteFont spriteFont;

        private readonly string text;

        public SpriteFontJob(ISpriteFont spriteFont, string text, Color color)
        {
            this.spriteFont = spriteFont;
            this.text = text;
            this.color = color;
        }

        public void Draw(ISpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.spriteFont, this.text, Vector2.Zero, this.color);
        }
    }
}