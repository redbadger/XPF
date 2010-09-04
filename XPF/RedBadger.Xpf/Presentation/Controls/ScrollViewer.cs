namespace RedBadger.Xpf.Presentation.Controls
{
    using RedBadger.Xpf.Presentation.Controls.Primitives;
    using RedBadger.Xpf.Presentation.Input;

    public class ScrollViewer : ContentControl, IInputElement
    {
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
            }
            else
            {
                var scrollContentPresenter = new ScrollContentPresenter();

                this.Content = scrollContentPresenter;
                scrollContentPresenter.Content = newContent;

                this.scrollInfo = scrollContentPresenter;
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