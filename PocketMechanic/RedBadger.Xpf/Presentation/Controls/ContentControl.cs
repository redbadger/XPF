namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;

    using Rect = RedBadger.Xpf.Presentation.Rect;
    using Size = RedBadger.Xpf.Presentation.Size;

#if WINDOWS_PHONE
    using UIElement = RedBadger.Xpf.Presentation.UIElement;
#endif

    /// <summary>
    ///   Represents a control with a single piece of content.
    /// </summary>
    public class ContentControl : UIElement
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", 
            typeof(IElement), 
            typeof(ContentControl), 
            new PropertyMetadata(null, ContentPropertyChangedCallback));

        public IElement Content
        {
            get
            {
                return (IElement)this.GetValue(ContentProperty);
            }

            set
            {
                this.SetValue(ContentProperty, value);
            }
        }

        public override void Render(ISpriteBatch spriteBatch)
        {
            IElement content = this.Content;
            if (content != null)
            {
                content.Render(spriteBatch);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            IElement content = this.Content;
            if (content != null)
            {
                content.Arrange(new Rect(finalSize));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            IElement content = this.Content;
            if (content == null)
            {
                return Size.Empty;
            }

            content.Measure(availableSize);
            return content.DesiredSize;
        }

        private static void ContentPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var child = args.NewValue as IElement;
            var parent = (IElement)dependencyObject;

            if (child != null)
            {
                child.VisualParent = parent;
            }
        }
    }
}