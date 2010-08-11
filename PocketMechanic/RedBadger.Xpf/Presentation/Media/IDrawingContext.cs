namespace RedBadger.Xpf.Presentation.Media
{
    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;

    public interface IDrawingContext
    {
        void DrawRectangle(Rect rect, Brush brush);

        void DrawText(ISpriteFont spriteFont, string text, Brush brush);

        void DrawText(ISpriteFont spriteFont, string text, Vector2 position, Brush brush);
    }
}