namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Windows;

    using RedBadger.Xpf.Presentation.Input;

    public abstract class ButtonBase : ContentControl
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

        internal void RaiseMouseLeftButtonUp()
        {
            this.OnMouseLeftButtonUp(new MouseButtonEventArgs());
        }

        internal void RaiseMouseLeftButtonDown()
        {
            this.OnMouseLeftButtonDown(new MouseButtonEventArgs());
        }

        protected virtual void OnClick()
        {
            EventHandler<EventArgs> handler = this.Click;
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