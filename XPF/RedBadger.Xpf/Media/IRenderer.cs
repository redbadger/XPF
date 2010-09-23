namespace RedBadger.Xpf.Media
{
    public interface IRenderer
    {
        void ClearInvalidDrawingContexts();

        void Draw();

        IDrawingContext GetDrawingContext(IElement element);

        void PreDraw();
    }
}