namespace RedBadger.Xpf
{
    using System.Collections.Generic;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Input;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    public interface IElement : IReactiveObject
    {
        object DataContext { get; set; }

        Size DesiredSize { get; }

        Subject<Gesture> Gestures { get; }

        double Height { get; set; }

        /// <summary>
        ///     Gets a value indicating whether the computed size and position of child elements in this element's layout are valid.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the size and position of layout are valid; otherwise, <c>false</c>.
        /// </value>
        bool IsArrangeValid { get; }

        bool IsMeasureValid { get; }

        Thickness Margin { get; set; }

        Vector VisualOffset { get; }

        IElement VisualParent { get; set; }

        double Width { get; set; }

        /// <summary>
        ///     Positions child elements and determines a size for a UIElement.
        ///     Parent elements call this method from their ArrangeOverride implementation to form a recursive layout update.
        ///     This method constitutes the second pass of a layout update.
        /// </summary>
        /// <param name = "finalRect">The final size that the parent computes for the child element, provided as a Rect instance.</param>
        void Arrange(Rect finalRect);

        IEnumerable<IElement> GetVisualChildren();

        bool HitTest(Point point);

        void InvalidateArrange();

        void InvalidateMeasure();

        /// <summary>
        ///     Updates the DesiredSize of a UIElement.
        ///     Derrived elements call this method from their own MeasureOverride implementations to form a recursive layout update.
        ///     Calling this method constitutes the first pass (the "Measure" pass) of a layout update.
        /// </summary>
        /// <param name = "availableSize">
        ///     The available space that a parent element can allocate a child element.
        ///     A child element can request a larger space than what is available; the provided size might be accommodated.
        /// </param>
        void Measure(Size availableSize);

        bool TryGetRenderer(out IRenderer renderer);

        bool TryGetRootElement(out IRootElement rootElement);
    }
}