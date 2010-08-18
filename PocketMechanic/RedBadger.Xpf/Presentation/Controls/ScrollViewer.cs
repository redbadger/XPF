namespace RedBadger.Xpf.Presentation.Controls
{
    using RedBadger.Xpf.Presentation.Controls.Primitives;

    public class ScrollViewer : ContentControl
    {
        protected override void OnContentChanged(IElement oldContent, IElement newContent)
        {
            if (newContent is IScrollInfo)
            {
                return;
            }

            this.Content = new ScrollContentPresenter { Content = newContent };
        }
    }
}