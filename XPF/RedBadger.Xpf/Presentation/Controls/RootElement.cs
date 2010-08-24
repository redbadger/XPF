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
            License.Validate();

            if (renderer == null)
            {
                throw new ArgumentNullException("renderer");
            }

            this.viewPort = viewPort;
            this.renderer = renderer;
            this.inputManager = inputManager;

            if (this.inputManager != null)
            {
                this.inputManager.Gestures.Subscribe(this.Gestures);
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

        public void ReleaseMouseCapture(IElement element)
        {
            if (this.elementWithMouseCapture == element)
            {
                this.elementWithMouseCapture = null;
            }
        }

        protected override void OnNextGesture(Gesture gesture)
        {
            if (this.elementWithMouseCapture != null)
            {
                this.elementWithMouseCapture.Gestures.OnNext(gesture);
            }
            else
            {
                if (!OnNextGestureFindChild(this, gesture))
                {
                    OnNextGestureFindElement(this, gesture);
                }
            }
        }

        private static bool OnNextGestureFindChild(IElement element, Gesture gesture)
        {
            return
                element.GetVisualChildren().Reverse().Where(child => !OnNextGestureFindChild(child, gesture)).Any(
                    child => OnNextGestureFindElement(child, gesture));
        }

        private static bool OnNextGestureFindElement(IElement element, Gesture gesture)
        {
            if (element is IInputElement && element.HitTest(gesture.Point))
            {
                element.Gestures.OnNext(gesture);
                return true;
            }

            return false;
        }
    }
}