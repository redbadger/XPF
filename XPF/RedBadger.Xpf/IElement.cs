#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

namespace RedBadger.Xpf
{
    using System.Collections.Generic;
    
    using System.Reactive.Subjects;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Input;

    public interface IElement : IReactiveObject
    {
        Rect ClippingRect { get; }

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

        /// <summary>
        ///     Returns the immediate visual children of the current <see cref = "IElement">IElement</see>.
        /// </summary>
        /// <returns><see cref = "IEnumerable{T}">IEnumerable</see> of immediate visual children.</returns>
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
