namespace RedBadger.Xpf.Presentation
{
    public interface IElement
    {
        Size DesiredSize { get; }

        void Measure(Size availableSize);
    }
}