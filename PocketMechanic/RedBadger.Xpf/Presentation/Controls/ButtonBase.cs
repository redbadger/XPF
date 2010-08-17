namespace RedBadger.Xpf.Presentation.Controls
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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.isMouseLeftButtonDown = true;

            if (!this.IsEnabled)
            {
                return;
            }

            this.CaptureMouse();
            if (this.isMouseCaptured)
            {
                this.IsPressed = true;
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            this.isMouseLeftButtonDown = false;

            if (!this.IsEnabled)
            {
                return;
            }

            if (this.IsPressed)
            {
                this.OnClick();
            }

            this.ReleaseMouseCapture();
            this.IsPressed = false;
        }

        protected override void OnMouseMove(MouseButtonEventArgs mouseButtonEventArgs)
        {
            base.OnMouseMove(mouseButtonEventArgs);

            if (this.isMouseLeftButtonDown && this.IsEnabled && this.isMouseCaptured)
            {
                this.IsPressed = this.HitTest(mouseButtonEventArgs.Position);
            }
        }

        private void CaptureMouse()
        {
            IRootElement rootElement;
            if (!this.isMouseCaptured && this.TryGetRootElement(out rootElement))
            {
                this.isMouseCaptured = rootElement.CaptureMouse(this);
            }
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