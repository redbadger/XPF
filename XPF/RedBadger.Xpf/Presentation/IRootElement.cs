namespace RedBadger.Xpf.Presentation
{
    using RedBadger.Xpf.Presentation.Input;
    using RedBadger.Xpf.Presentation.Media;

    public interface IRootElement
    {
        IInputManager InputManager { get; }

        IRenderer Renderer { get; }

        bool CaptureMouse(IElement element);

        void ReleaseMouseCapture(IElement element);
    }
}