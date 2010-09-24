namespace RedBadger.Xpf.Graphics
{
    using RedBadger.Xpf.Media;

    public interface IDrawingContext
    {
        Rect AbsoluteClippingRect { get; set; }

        // This isn't used via the interface.
        Vector AbsoluteOffset { get; }

        // Should this move onto IElement to mirror Offset?
        Rect ClippingRect { get; set; }

        IElement Element { get; }

        void DrawImage(ImageSource imageSource, Rect rect);

        void DrawRectangle(Rect rect, Brush brush);

        void DrawText(ISpriteFont spriteFont, string text, Point position, Brush brush);
    }
}