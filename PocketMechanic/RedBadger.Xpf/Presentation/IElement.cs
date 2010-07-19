namespace RedBadger.Xpf.Presentation
{
    public interface IElement
    {
        Size DesiredSize { get; }

        void Draw();

        void Measure(Size availableSize);
    }
}