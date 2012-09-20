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

namespace RedBadger.Xpf.Controls
{
    using RedBadger.Xpf.Controls.Primitives;
    using RedBadger.Xpf.Input;

    public class ScrollViewer : ContentControl, IInputElement
    {
        private bool isInsertingScrollContentPresenter;

        private IScrollInfo scrollInfo;

        public bool CanHorizontallyScroll
        {
            get
            {
                return this.scrollInfo != null ? this.scrollInfo.CanHorizontallyScroll : false;
            }

            set
            {
                if (this.scrollInfo != null)
                {
                    this.scrollInfo.CanHorizontallyScroll = value;
                }
            }
        }

        public bool CanVerticallyScroll
        {
            get
            {
                return this.scrollInfo != null ? this.scrollInfo.CanVerticallyScroll : false;
            }

            set
            {
                if (this.scrollInfo != null)
                {
                    this.scrollInfo.CanVerticallyScroll = value;
                }
            }
        }

        public Size Extent
        {
            get
            {
                return this.scrollInfo != null ? this.scrollInfo.Extent : new Size();
            }
        }

        public Size Viewport
        {
            get
            {
                return this.scrollInfo != null ? this.scrollInfo.Viewport : new Size();
            }
        }

        protected override void OnContentChanged(IElement oldContent, IElement newContent)
        {
            var oldScrollContentPresenter = oldContent as ScrollContentPresenter;
            if (oldScrollContentPresenter != null)
            {
                oldScrollContentPresenter.Content.VisualParent = null;
            }

            var newScrollInfo = newContent as IScrollInfo;
            if (newScrollInfo != null)
            {
                this.scrollInfo = newScrollInfo;

                if (oldContent != null && this.isInsertingScrollContentPresenter)
                {
                    ((ScrollContentPresenter)newContent).Content = oldContent;
                }

                this.isInsertingScrollContentPresenter = false;
            }
            else
            {
                this.isInsertingScrollContentPresenter = true;
                this.Content = new ScrollContentPresenter();
            }
        }

        protected override void OnNextGesture(Gesture gesture)
        {
            switch (gesture.Type)
            {
                case GestureType.LeftButtonDown:
                    this.CaptureMouse();
                    break;
                case GestureType.FreeDrag:
                    if (this.scrollInfo != null && this.IsMouseCaptured)
                    {
                        this.scrollInfo.SetHorizontalOffset(this.scrollInfo.Offset.X - gesture.Delta.X);
                        this.scrollInfo.SetVerticalOffset(this.scrollInfo.Offset.Y - gesture.Delta.Y);
                    }

                    break;
                case GestureType.LeftButtonUp:
                    if (this.IsMouseCaptured)
                    {
                        this.ReleaseMouseCapture();
                    }

                    break;
            }
        }
    }
}
