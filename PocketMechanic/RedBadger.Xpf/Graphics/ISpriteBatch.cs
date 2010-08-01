namespace RedBadger.Xpf.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface ISpriteBatch
    {
        void Draw(Texture2D texture, Vector2 position, Color color);

        void Draw(Texture2D texture, Rectangle area, Color color);

        void Draw(ITexture2D texture2D, Vector2 position, Color color);

        void Draw(ITexture2D texture2D, Rectangle area, Color color);

        void Draw(
            Texture2D texture, 
            Vector2 position, 
            Rectangle? source, 
            Color color, 
            float rotation, 
            Vector2 origin, 
            float scale, 
            SpriteEffects effects, 
            float depth);

        void Draw(
            Texture2D texture, 
            Vector2 position, 
            Rectangle? source, 
            Color color, 
            float rotation, 
            Vector2 origin, 
            Vector2 scale, 
            SpriteEffects effects, 
            float depth);

        void DrawString(ISpriteFont spriteFont, string text, Vector2 position, Color color);

        void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color);

        void DrawString(
            ISpriteFont spriteFont, 
            string text, 
            Vector2 position, 
            Color color, 
            float rotation, 
            Vector2 origin, 
            float scale, 
            SpriteEffects effects, 
            float layerDepth);
    }
}