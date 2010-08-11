namespace RedBadger.Xpf.Presentation.Media
{
    public interface IRenderer
    {
        void Draw();

        IDrawingContext GetDrawingContext(IElement element);

        void PreDraw();
    }
}