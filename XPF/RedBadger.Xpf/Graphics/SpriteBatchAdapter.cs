namespace RedBadger.Xpf.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Presentation;

    using Color = RedBadger.Xpf.Presentation.Media.Color;

    public class SpriteBatchAdapter : SpriteBatch, ISpriteBatch
    {
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