namespace RedBadger.Xpf.Controls.Primitives
{
    using System;

    public class ToggleButton : ButtonBase
    {
        public static readonly ReactiveProperty<bool?> IsCheckedProperty = ReactiveProperty<bool?>.Register(
            "IsChecked", typeof(ToggleButton), false, OnIsCheckedPropertyChanged);

        public static readonly ReactiveProperty<bool> IsThreeStateProperty =
            ReactiveProperty<bool>.Register("IsThreeState", typeof(ToggleButton));

        public event EventHandler<EventArgs> Checked;

        public event EventHandler<EventArgs> Indeterminate;

        public event EventHandler<EventArgs> Unchecked;

        public bool? IsChecked
        {
            get
            {
                return this.GetValue(IsCheckedProperty);
            }

            set
            {
                this.SetValue(IsCheckedProperty, value);
            }
        }

        public bool IsThreeState
        {
            get
            {
                return this.GetValue(IsThreeStateProperty);
            }

            set
            {
                this.SetValue(IsThreeStateProperty, value);
            }
        }

        protected internal virtual void OnToggle()
        {
            bool? isChecked = this.IsChecked;

            if (isChecked == true)
            {
                this.IsChecked = this.IsThreeState ? (bool?)null : false;
            }
            else
            {
                this.IsChecked = new bool?(isChecked.HasValue);
            }
        }

        protected virtual void OnChecked()
        {
            EventHandler<EventArgs> eventHandler = this.Checked;
            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        protected override void OnClick()
        {
            this.OnToggle();
            base.OnClick();
        }

        protected virtual void OnIndeterminate()
        {
            EventHandler<EventArgs> eventHandler = this.Indeterminate;
            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnUnchecked()
        {
            EventHandler<EventArgs> eventHandler = this.Unchecked;
            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        private static void OnIsCheckedPropertyChanged(
            IReactiveObject source, ReactivePropertyChangeEventArgs<bool?> args)
        {
            var button = source as ToggleButton;
            if (button == null)
            {
                return;
            }

            bool? newValue = args.NewValue;
            if (newValue == true)
            {
                button.OnChecked();
            }
            else if (newValue == false)
            {
                button.OnUnchecked();
            }
            else
            {
                button.OnIndeterminate();
            }
        }
    }
}