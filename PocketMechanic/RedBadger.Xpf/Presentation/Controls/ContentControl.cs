namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    ///     Represents a control with a single piece of content.
    /// </summary>
    public class ContentControl : Control
    {
        public static readonly XpfDependencyProperty ContentProperty = XpfDependencyProperty.Register(
            "Content", 
            typeof(IElement), 
            typeof(ContentControl), 
            new PropertyMetadata(null, ContentPropertyChangedCallback));

        public IElement Content
        {
            get
            {
                return (IElement)this.GetValue(ContentProperty.Value);
            }

            set
            {
                this.SetValue(ContentProperty.Value, value);
            }
        }

        public override IEnumerable<IElement> GetVisualChildren()
        {
            var content = this.Content;
            if (content != null)
            {
                yield return content;
            }

            yield break;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var content = this.Content;
            if (content != null)
            {
                content.Arrange(new Rect(new Point(), finalSize));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var content = this.Content;
            if (content == null)
            {
                return Size.Empty;
            }

            content.Measure(availableSize);
            return content.DesiredSize;
        }

        protected virtual void OnContentChanged(IElement oldContent, IElement newContent)
        {
        }

        private static void ContentPropertyChangedCallback(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var oldContent = args.OldValue as IElement;
            var newContent = args.NewValue as IElement;
            var contentControl = (ContentControl)dependencyObject;

            contentControl.InvalidateMeasure();

            if (oldContent != null)
            {
                oldContent.VisualParent = null;
            }

            if (newContent != null)
            {
                newContent.VisualParent = contentControl;
            }

            contentControl.OnContentChanged(oldContent, newContent);
        }
    }
}