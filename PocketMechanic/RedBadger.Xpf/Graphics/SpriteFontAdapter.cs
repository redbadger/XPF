namespace RedBadger.Xpf.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class SpriteFontAdapter : ISpriteFont
    {
        private readonly SpriteFont spriteFont;

        public SpriteFontAdapter(SpriteFont spriteFont)
        {
            this.spriteFont = spriteFont;
        }

        public SpriteFont Value
        {
            get
            {
                return this.spriteFont;
            }
        }

        public Vector2 MeasureString(string text)
        {
            return this.spriteFont.MeasureString(text ?? string.Empty);
        }
    }
}
