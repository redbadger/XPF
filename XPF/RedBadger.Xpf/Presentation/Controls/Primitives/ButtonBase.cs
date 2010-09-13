namespace RedBadger.Xpf.Presentation.Controls.Primitives
{
    using System;

    using RedBadger.Xpf.Presentation.Input;

    public abstract class ButtonBase : ContentControl, IInputElement
    {
        public static readonly ReactiveProperty<bool, ButtonBase> IsPressedProperty =
            ReactiveProperty<bool, ButtonBase>.Register("IsPressed", false);

        private bool isLeftButtonDown;

        public event EventHandler<EventArgs> Click;

        public bool IsPressed
        {
            get
            {
                return this.GetValue(IsPressedProperty);
            }

            protected internal set
            {
                this.SetValue(IsPressedProperty, value);
            }
        }

        protected virtual void OnClick()
        {
            EventHandler<EventArgs> handler = this.Click;
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