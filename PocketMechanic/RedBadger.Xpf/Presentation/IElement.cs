namespace RedBadger.Xpf.Presentation
{
    using RedBadger.Xpf.Graphics;

    public interface IElement
    {
        Size DesiredSize { get; }

        IElement VisualParent { get; set; }

        /// <summary>
        ///   Gets a value indicating whether the computed size and position of child elements in this element's layout are valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the size and position of layout are valid; otherwise, <c>false</c>.
        /// </value>
        bool IsArrangeValid { get; }

        bool IsMeasureValid { get; }

        void Measure(Size availableSize);

        void Render(ISpriteBatch spriteBatch);

        /// <summary>
        ///   Positions child elements and determines a size for a UIElement.
        ///   Parent elements call this method from their ArrangeOverride implementation to form a recursive layout update.
        ///   This method constitutes the second pass of a layout update.
        /// </summary>
        /// <param name = "finalRect">The final size that the parent computes for the child element, provided as a Rect instance.</param>
        void Arrange(Rect finalRect);

        void InvalidateArrange();

        void InvalidateMeasure();
    }
}