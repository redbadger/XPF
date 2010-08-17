namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Linq;
    using System.Windows;

    using RedBadger.Xpf.Presentation.Input;
    using RedBadger.Xpf.Presentation.Media;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

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
                this.inputManager.MouseData.Where(
                    data => data.Action == MouseAction.LeftButtonDown || data.Action == MouseAction.LeftButtonUp).
                    Subscribe(Observer.Create<MouseData>(this.OnNextMouseUpDownInternal));
                this.inputManager.MouseData.Where(data => data.Action == MouseAction.Move).Subscribe(
                    Observer.Create<MouseData>(this.OnNextMouseMoveInternal));
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

        private void OnNextMouseMoveInternal(MouseData mouseData)
        {
            if (this.elementWithMouseCapture != null)
            {
                this.elementWithMouseCapture.OnNextMouseMove(mouseData);
            }
            else
            {
                this.OnNextMouseMove(mouseData);
            }
        }

        private void OnNextMouseUpDownInternal(MouseData mouseData)
        {
            if (this.elementWithMouseCapture != null)
            {
                this.elementWithMouseCapture.OnNextMouseUpDown(mouseData);
            }
            else
            {
                this.OnNextMouseUpDown(mouseData);
            }
        }
    }
}