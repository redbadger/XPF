namespace RedBadger.Xpf.Presentation.Input
{
    public interface IInputElement
    {
        bool IsMouseCaptured { get; }

        bool CaptureMouse();

        void ReleaseMouseCapture();
    }
}