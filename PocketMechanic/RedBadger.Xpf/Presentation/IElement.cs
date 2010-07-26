namespace RedBadger.Xpf.Presentation
{
    using RedBadger.Xpf.Graphics;

    public interface IElement
    {
        Size DesiredSize { get; }

        void Measure(Size availableSize);

        void Render(ISpriteBatch spriteBatch);

        /// <summary>
        ///   Positions child elements and determines a size for a UIElement.
        ///   Parent elements call this method from their ArrangeOverride implementation to form a recursive layout update.
        ///   This method constitutes the second pass of a layout update.
        /// </summary>
        /// <param name = "finalRect">The final size that the parent computes for the child element, provided as a Rect instance.</param>
        void Arrange(Rect finalRect);
    }
}