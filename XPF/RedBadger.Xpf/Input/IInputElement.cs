namespace RedBadger.Xpf.Input
{
    public interface IInputElement
    {
        bool IsMouseCaptured { get; }

        bool CaptureMouse();

        void ReleaseMouseCapture();
    }
}