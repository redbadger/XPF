namespace RedBadger.Xpf.Controls.Primitives
{
    using System;

    /// <summary>
    ///     Base class for all controls with toggling functionality.
    /// </summary>
    public class ToggleButton : ButtonBase
    {
        /// <summary>
        ///     <see cref = "IsChecked">IsChecked</see> Reactive Property.
        /// </summary>
        public static readonly ReactiveProperty<bool?> IsCheckedProperty = ReactiveProperty<bool?>.Register(
            "IsChecked", typeof(ToggleButton), false, OnIsCheckedPropertyChanged);

        /// <summary>
        ///     <see cref = "IsThreeState">IsThreeState</see> Reactive Property.
        /// </summary>
        public static readonly ReactiveProperty<bool> IsThreeStateProperty =
            ReactiveProperty<bool>.Register("IsThreeState", typeof(ToggleButton));

        /// <summary>
        ///     Occurs when a <see cref = "ToggleButton">ToggleButton</see> is checked.
        /// </summary>
        public event EventHandler<EventArgs> Checked;

        /// <summary>
        ///     Occurs when the state of a <see cref = "ToggleButton">ToggleButton</see> is neither checked nor unchecked.
        /// </summary>
        /// <remarks>
        ///     This can only occur when <see cref = "IsThreeState">IsThreeState</see> is true.
        /// </remarks>
        public event EventHandler<EventArgs> Indeterminate;

        /// <summary>
        ///     Occurs when a <see cref = "ToggleButton">ToggleButton</see> is unchecked.
        /// </summary>
        public event EventHandler<EventArgs> Unchecked;

        /// <summary>
        ///     Gets or sets a value indicating whether a <see cref = "ToggleButton">ToggleButton</see> is in a checked, unchecked or indeterminate state.
        /// </summary>
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

        /// <summary>
        ///     Gets or sets a value indicating whether a <see cref = "ToggleButton">ToggleButton</see> supports two or three states.
        /// </summary>
        /// <remarks>
        ///     If false, <see cref = "IsChecked">IsChecked</see> can only be true or false.  If true, <see cref = "IsChecked">IsChecked</see> can enter a third null state.
        /// </remarks>
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