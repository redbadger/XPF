namespace RedBadger.Xpf.Presentation.Controls
{
    using RedBadger.Xpf.Presentation.Controls.Primitives;
    using RedBadger.Xpf.Presentation.Input;

    public class ScrollViewer : ContentControl, IInputElement
    {
        protected override void OnContentChanged(IElement oldContent, IElement newContent)
        {
            if (newContent is IScrollInfo)
            {
                return;
            }

            this.Content = new ScrollContentPresenter { Content = newContent };
        }

        protected override void OnNextGesture(Gesture gesture)
        {
            switch (gesture.Type)
            {
                case GestureType.FreeDrag:
                    this.CaptureMouse();
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