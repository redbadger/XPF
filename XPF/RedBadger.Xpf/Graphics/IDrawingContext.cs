namespace RedBadger.Xpf.Graphics
{
    using RedBadger.Xpf.Media;

    public interface IDrawingContext
    {
        Rect ClippingRect { get; set; }

        IElement Element { get; }

        Vector AbsoluteOffset { get; }

        void DrawImage(ImageSource imageSource, Rect rect);

        void DrawRectangle(Rect rect, Brush brush);

        void DrawText(ISpriteFont spriteFont, string text, Point position, Brush brush);
    }
}