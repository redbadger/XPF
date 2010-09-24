namespace RedBadger.Xpf.Graphics
{
    using RedBadger.Xpf.Media;

    public interface IDrawingContext
    {
        Rect AbsoluteClippingRect { get; set; }

        Vector AbsoluteOffset { get; }

        Rect ClippingRect { get; set; }

        IElement Element { get; }

        IDrawingContext Next { get; set; }

        void DrawImage(ImageSource imageSource, Rect rect);

        void DrawRectangle(Rect rect, Brush brush);

        void DrawText(ISpriteFont spriteFont, string text, Point position, Brush brush);
    }
}