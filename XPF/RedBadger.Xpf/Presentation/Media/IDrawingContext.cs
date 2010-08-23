namespace RedBadger.Xpf.Presentation.Media
{
    using System.Windows;

    using RedBadger.Xpf.Graphics;

    using Vector = RedBadger.Xpf.Presentation.Vector;

    public interface IDrawingContext
    {
        void DrawImage(ImageSource imageSource, Rect rect);

        void DrawRectangle(Rect rect, Brush brush);

        void DrawText(ISpriteFont spriteFont, string text, Vector position, Brush brush);
    }
}