#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

namespace RedBadger.Xpf.Controls.Primitives
{
    using System;

    using RedBadger.Xpf.Input;

    public abstract class ButtonBase : ContentControl, IInputElement
    {
        public static readonly ReactiveProperty<bool> IsPressedProperty = ReactiveProperty<bool>.Register(
            "IsPressed", typeof(ButtonBase), false);

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
