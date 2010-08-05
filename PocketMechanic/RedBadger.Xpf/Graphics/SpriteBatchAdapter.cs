namespace RedBadger.Xpf.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class SpriteBatchAdapter : SpriteBatch, ISpriteBatch
    {
        public SpriteBatchAdapter(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
        }

        public void DrawString(ISpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            this.DrawString(spriteFont.Value, text ?? string.Empty, position, color);
        }

        public void DrawString(ISpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            this.DrawString(spriteFont.Value, text ?? string.Empty, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public void Draw(ITexture2D texture2D, Vector2 position, Color color)
        {
            if (texture2D != null)
            {
                this.Draw(texture2D.Value, position, color);
            }
        }

        public void Draw(ITexture2D texture2D, Rectangle area, Color color)
        {
            if (texture2D != null)
            {
                this.Draw(texture2D.Value, area, color);
            }
        }
    }
}
