namespace RedBadger.Xpf.Presentation.Controls.Primitives
{
    using System;
    using System.Windows;

    using RedBadger.Xpf.Presentation.Input;

    using IInputElement = RedBadger.Xpf.Presentation.Input.IInputElement;

    public abstract class ButtonBase : ContentControl, IInputElement
    {
        public static readonly XpfDependencyProperty IsPressedProperty = XpfDependencyProperty.Register(
            "IsPressed", typeof(bool), typeof(ButtonBase), new PropertyMetadata(false));

        private bool isMouseCaptured;

        private bool isMouseLeftButtonDown;

        public event EventHandler<EventArgs> Click;

        public bool IsPressed
        {
            get
            {
                return (bool)this.GetValue(IsPressedProperty.Value);
            }

            protected internal set
            {
                this.SetValue(IsPressedProperty.Value, value);
            }
        }

        protected virtual void OnClick()
        {
            var handler = this.Click;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        protected override void OnNextMouseData(MouseData mouseData)
        {
            if (!this.IsEnabled)
            {
                return;
            }

            switch (mouseData.Action)
            {
                case MouseAction.LeftButtonDown:
                    this.isMouseLeftButtonDown = true;

                    if (this.CaptureMouse())
                    {
                        this.IsPressed = true;
                    }

                    break;
                case MouseAction.LeftButtonUp:
                    this.isMouseLeftButtonDown = false;

                    if (this.IsPressed)
                    {
                        this.OnClick();
                    }

                    this.ReleaseMouseCapture();
                    this.IsPressed = false;
                    break;
                case MouseAction.Move:
                    if (this.isMouseLeftButtonDown && this.isMouseCaptured)
                    {
                        this.IsPressed = this.HitTest(mouseData.Point);
                    }

                    break;
            }
        }

        private bool CaptureMouse()
        {
            IRootElement rootElement;
            if (!this.isMouseCaptured && this.TryGetRootElement(out rootElement))
            {
                this.isMouseCaptured = rootElement.CaptureMouse(this);
                return this.isMouseCaptured;
            }

            return false;
        }

        private void ReleaseMouseCapture()
        {
            IRootElement rootElement;
            if (this.TryGetRootElement(out rootElement))
            {
                rootElement.ReleaseMouseCapture();
            }

            this.isMouseCaptured = false;
        }
    }
}