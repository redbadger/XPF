namespace RedBadger.Xpf.Graphics
{
    public interface IRenderer
    {
        void ClearInvalidDrawingContexts();

        void Draw();

        IDrawingContext GetDrawingContext(IElement element);

        void PreDraw(IElement rootElement);
    }
}