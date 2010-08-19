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

        private bool isLeftButtonDown;

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

        protected override void OnNextGesture(Gesture gesture)
        {
            if (!this.IsEnabled)
            {
                return;
            }

            switch (gesture.Type)
            {
                case GestureType.LeftButtonDown:
                    this.isLeftButtonDown = true;

                    if (this.CaptureMouse())
                    {
                        this.IsPressed = true;
                    }

                    break;
                case GestureType.LeftButtonUp:
                    this.isLeftButtonDown = false;

                    if (this.IsPressed)
                    {
                        this.OnClick();
                    }

                    this.ReleaseMouseCapture();
                    this.IsPressed = false;
                    break;
                case GestureType.Move:
                    if (this.isLeftButtonDown && this.IsMouseCaptured)
                    {
                        this.IsPressed = this.HitTest(gesture.Point);
                    }

                    break;
            }
        }
    }
}