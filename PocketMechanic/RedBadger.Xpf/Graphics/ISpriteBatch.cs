namespace RedBadger.Xpf.Graphics
{
    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Presentation;

    public interface ISpriteBatch
    {
        void Draw(ITexture2D texture2D, Rect rect, Color color);

        void DrawString(ISpriteFont spriteFont, string text, Vector2 position, Color color);
    }
}