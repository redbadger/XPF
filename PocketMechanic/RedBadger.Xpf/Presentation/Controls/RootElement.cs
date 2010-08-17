namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using RedBadger.Xpf.Presentation.Input;
    using RedBadger.Xpf.Presentation.Media;

    public class RootElement : ContentControl, IRootElement
    {
        private readonly InputManager inputManager;

        private readonly IRenderer renderer;

        private readonly Rect viewPort;

        public RootElement(Rect viewPort, IRenderer renderer)
            : this(viewPort, renderer, null)
        {
        }

        public RootElement(Rect viewPort, IRenderer renderer, IMouse mouse)
        {
            this.renderer = renderer;
            this.viewPort = viewPort;

            if (mouse != null)
            {
                this.inputManager = new InputManager(this, mouse);
            }
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

            this.Measure(new Size(this.viewPort.Width, this.viewPort.Height));
            this.Arrange(this.viewPort);
            this.renderer.PreDraw();

            if (this.inputManager != null)
            {
                this.inputManager.Update();
            }
        }
    }
}