namespace RedBadger.Xpf.Presentation.Controls
{
    public class RootElement : ContentControl
    {
        private readonly Rect viewPort;

        public RootElement(Rect viewPort)
        {
            this.viewPort = viewPort;
        }

        public void Update()
        {
            this.Measure(this.viewPort.Size);
            this.Arrange(this.viewPort);
        }
    }
}