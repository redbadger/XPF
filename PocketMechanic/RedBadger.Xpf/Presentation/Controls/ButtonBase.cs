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

            if (!this.IsEnabled)
            {
                return;
            }

            this.IsPressed = true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (!this.IsEnabled)
            {
                return;
            }

            if (this.IsPressed)
            {
                this.OnClick();
                this.IsPressed = false;
            }
        }
    }
}