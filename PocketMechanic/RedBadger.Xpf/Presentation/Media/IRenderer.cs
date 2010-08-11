namespace RedBadger.Xpf.Presentation.Media
{
    using RedBadger.Xpf.Graphics;

    public interface IRenderer
    {
        void Clear();

        void Draw(ISpriteBatch spriteBatch);

        IDrawingContext GetDrawingContext(IElement element);

        void PreDraw();
    }
}