namespace RedBadger.Xpf.Presentation
{
    using System.Collections.Generic;

    using RedBadger.Xpf.Presentation.Input;
    using RedBadger.Xpf.Presentation.Media;

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

        IElement VisualParent { get; set; }

        double Width { get; set; }

        /// <summary>
        ///     Positions child elements and determines a size for a UIElement.
        ///     Parent elements call this method from their ArrangeOverride implementation to form a recursive layout update.
        ///     This method constitutes the second pass of a layout update.
        /// </summary>
        /// <param name = "finalRect">The final size that the parent computes for the child element, provided as a Rect instance.</param>
        void Arrange(Rect finalRect);

        Vector CalculateAbsoluteOffset();

        IEnumerable<IElement> GetVisualChildren();

        bool HitTest(Point point);

        void InvalidateArrange();

        void InvalidateMeasure();

        void Measure(Size availableSize);

        bool TryGetRenderer(out IRenderer renderer);

        bool TryGetRootElement(out IRootElement rootElement);
    }
}