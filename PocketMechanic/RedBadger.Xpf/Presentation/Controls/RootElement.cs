namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Linq;
    using System.Windows;

    using RedBadger.Xpf.Presentation.Input;
    using RedBadger.Xpf.Presentation.Media;

    using IInputElement = RedBadger.Xpf.Presentation.Input.IInputElement;

    public class RootElement : ContentControl, IRootElement
    {
        private readonly IInputManager inputManager;

        private readonly IRenderer renderer;

        private readonly Rect viewPort;

        private IElement elementWithMouseCapture;

        public RootElement(Rect viewPort, IRenderer renderer)
            : this(viewPort, renderer, null)
        {
        }

        public RootElement(Rect viewPort, IRenderer renderer, IInputManager inputManager)
        {
            if (renderer == null)
            {
                throw new ArgumentNullException("renderer");
            }

            this.viewPort = viewPort;
            this.renderer = renderer;
            this.inputManager = inputManager;

            if (this.inputManager != null)
            {
                this.inputManager.MouseData.Subscribe(this.MouseData);
            }
        }

        public IInputManager InputManager
        {
            get
            {
                return this.inputManager;
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

        public bool CaptureMouse(IElement element)
        {
            if (this.elementWithMouseCapture == null)
            {
                this.elementWithMouseCapture = element;
                return true;
            }

            return false;
        }

        public void ReleaseMouseCapture()
        {
            this.elementWithMouseCapture = null;
        }

        protected override void OnNextMouseData(MouseData mouseData)
        {
            if (this.elementWithMouseCapture != null)
            {
                this.elementWithMouseCapture.MouseData.OnNext(mouseData);
            }
            else
            {
                if (!OnNextMouseDataFindChild(this, mouseData))
                {
                    OnNextMouseDataFindElement(this, mouseData);
                }
            }
        }

        private static bool OnNextMouseDataFindChild(IElement element, MouseData mouseData)
        {
            return
                element.GetChildren().Reverse().Where(
                    child => !OnNextMouseDataFindChild(child, mouseData)).Any(
                        child => OnNextMouseDataFindElement(child, mouseData));
        }

        private static bool OnNextMouseDataFindElement(IElement element, MouseData mouseData)
        {
            if (element is IInputElement && element.HitTest(mouseData.Point))
            {
                element.MouseData.OnNext(mouseData);
                return true;
            }

            return false;
        }
    }
}