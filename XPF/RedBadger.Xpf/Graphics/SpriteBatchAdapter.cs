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
            if (texture2D != null)
            {
                var rectangle = new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
                this.Draw(texture2D.Value, rectangle, new Microsoft.Xna.Framework.Color(color.R, color.G, color.B));
            }
        }

        public void DrawString(ISpriteFont spriteFont, string text, Vector position, Color color)
        {
            this.DrawString(
                spriteFont.Value, 
                text ?? string.Empty, 
                new Vector2((float)position.X, (float)position.Y), 
                new Microsoft.Xna.Framework.Color(color.R, color.G, color.B));
        }
    }
}