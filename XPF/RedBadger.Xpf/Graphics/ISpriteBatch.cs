namespace RedBadger.Xpf.Graphics
{
    using RedBadger.Xpf.Media;

    public interface ISpriteBatch
    {
        void Draw(ITexture2D texture2D, Rect rect, Color color);

        void DrawString(ISpriteFont spriteFont, string text, Vector position, Color color);
    }
}