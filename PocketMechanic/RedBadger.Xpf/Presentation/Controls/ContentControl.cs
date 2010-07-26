namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;

    using Rect = RedBadger.Xpf.Presentation.Rect;
    using Size = RedBadger.Xpf.Presentation.Size;
    using UIElement = RedBadger.Xpf.Presentation.UIElement;

    /// <summary>
    ///   Represents a control with a single piece of content.
    /// </summary>
    public class ContentControl : UIElement
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(IElement), typeof(ContentControl), new PropertyMetadata(null));

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
                content.Arrange(new Rect(Vector2.Zero, finalSize));
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
    }
}