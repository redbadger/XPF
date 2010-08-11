namespace RedBadger.Xpf.Presentation.Controls
{
    using RedBadger.Xpf.Presentation.Media;

    public class RootElement : ContentControl, IRootElement
    {
        private readonly IRenderer renderer;

        private readonly Rect viewPort;

        private bool isFirst = true;

        public RootElement(IRenderer renderer, Rect viewPort)
        {
            this.renderer = renderer;
            this.viewPort = viewPort;
        }

        public IRenderer Renderer
        {
            get
            {
                return this.renderer;
            }
        }

        public void Draw()
        {
            this.renderer.Draw();
        }

        public void Update()
        {
            this.Measure(this.viewPort.Size);
            this.Arrange(this.viewPort);

            if (this.isFirst)
            {
                this.renderer.PreDraw();
                this.isFirst = false;
            }
        }
    }
}