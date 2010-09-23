namespace RedBadger.Xpf.Adapters.Xna.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;

    using Color = RedBadger.Xpf.Media.Color;

    /// <summary>
    /// Adapts an XNA <see cref="SpriteBatch">SpriteBatch</see> to an XPF <see cref="ISpriteBatch">ISpriteBatch</see>.
    /// </summary>
    public class SpriteBatchAdapter : SpriteBatch, ISpriteBatch
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="SpriteBatchAdapter">SpriteBatchAdapter</see>.
        /// </summary>
        /// <param name = "graphicsDevice">An XNA <see cref = "GraphicsDevice">GraphicsDevice</see> that can be used to generate primitives.</param>
        public SpriteBatchAdapter(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
        }

        public void Draw(ITexture2D texture2D, Rect rect, Color color)
        {
            var texture2DAdapter = texture2D as Texture2DAdapter;
            if (texture2DAdapter != null && texture2DAdapter.Value != null)
            {
                var rectangle = new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
                this.Draw(
                    texture2DAdapter.Value, 
                    rectangle, 
                    new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A));
            }
        }

        public void DrawString(ISpriteFont spriteFont, string text, Vector position, Color color)
        {
            var spriteFontAdapter = spriteFont as SpriteFontAdapter;
            if (spriteFontAdapter != null && spriteFontAdapter.Value != null)
            {
                this.DrawString(
                    spriteFontAdapter.Value, 
                    text ?? string.Empty, 
                    new Vector2((float)position.X, (float)position.Y), 
                    new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A));
            }
        }
    }
}