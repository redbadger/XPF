namespace RedBadger.Xpf.Presentation.Controls
{
    using RedBadger.Xpf.Presentation.Media;

    public class RootElement : ContentControl, IRootElement
    {
        private readonly IRenderer renderer;

        private readonly Rect viewPort;

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
            if (!this.IsArrangeValid)
            {
                this.renderer.ClearInvalidDrawingContexts();
            }

            this.Measure(this.viewPort.Size);
            this.Arrange(this.viewPort);
            this.renderer.PreDraw();
        }
    }
}