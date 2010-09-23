namespace RedBadger.Xpf.Adapters.Xna.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;

    /// <summary>
    ///     Adapts an XNA <see cref = "SpriteFont">SpriteFont</see> to an XPF <see cref = "ISpriteFont">ISpriteFont</see>.
    /// </summary>
    public class SpriteFontAdapter : ISpriteFont
    {
        private readonly SpriteFont spriteFont;

        /// <summary>
        ///     Initializes a new instance of a <see cref = "SpriteFontAdapter">SpriteFontAdapter</see>.
        /// </summary>
        /// <param name = "spriteFont">The XNA <see cref = "SpriteFont">SpriteFont</see> to adapt.</param>
        public SpriteFontAdapter(SpriteFont spriteFont)
        {
            this.spriteFont = spriteFont;
        }

        /// <summary>
        ///     The adapted XNA <see cref = "SpriteFont">SpriteFont</see>.
        /// </summary>
        public SpriteFont Value
        {
            get
            {
                return this.spriteFont;
            }
        }

        public Size MeasureString(string text)
        {
            Vector2 size = this.spriteFont.MeasureString(text ?? string.Empty);
            return new Size(size.X, size.Y);
        }
    }
}