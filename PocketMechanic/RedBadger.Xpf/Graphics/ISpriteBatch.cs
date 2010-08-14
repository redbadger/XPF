namespace RedBadger.Xpf.Graphics
{
    using System.Windows.Media;

    using RedBadger.Xpf.Presentation;

    public interface ISpriteBatch
    {
        void Draw(ITexture2D texture2D, Rect rect, Color color);

        void DrawString(ISpriteFont spriteFont, string text, Vector position, Color color);
    }
}