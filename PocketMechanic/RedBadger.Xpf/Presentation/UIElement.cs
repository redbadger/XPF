namespace RedBadger.Xpf.Presentation
{
    using System;

    using Microsoft.Xna.Framework;

    public abstract class UIElement : IElement
    {
        public Size DesiredSize { get; private set; }

        public Vector2 DrawPosition { get; set; }

        /// <summary>
        /// Gets a value indicating whether the computed size and position of child elements in this element's layout are valid.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the size and position of layout are valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsArrangeValid { get; private set; }

        public bool IsMeasureValid { get; private set; }

        public Thickness Margin
        {
            get; set;
        }

        public void Draw()
        {
        }

        /// <summary>
        /// Positions child elements and determines a size for a UIElement.
        /// Parent elements call this method from their ArrangeOverride implementation to form a recursive layout update.
        /// This method constitutes the second pass of a layout update.
        /// </summary>
        /// <param name="finalRect">The final size that the parent computes for the child element, provided as a Rect instance.</param>
        public void Arrange(Rect finalRect)
        {
        }

        /// <summary>
        /// Updates the DesiredSize of a UIElement.
        /// Derrived elements call this method from their own MeasureOverride implementations to form a recursive layout update.
        /// Calling this method constitutes the first pass (the "Measure" pass) of a layout update.
        /// </summary>
        /// <param name="availableSize">
        /// The available space that a parent element can allocate a child element.
        /// A child element can request a larger space than what is available; the provided size might be accommodated.
        /// </param>
        public void Measure(Size availableSize)
        {
            if (float.IsNaN(availableSize.Width) || float.IsNaN(availableSize.Height))
            {
                throw new InvalidOperationException("AvailableSize Width or Height cannot be NaN");
            }

            if (!this.IsMeasureValid)
            {
                var size = this.MeasureCore(availableSize);

                if (float.IsPositiveInfinity(size.Width) || float.IsPositiveInfinity(size.Height))
                {
                    throw new InvalidOperationException("The implementing element returned a PositiveInfinity");
                }

                if (float.IsNaN(size.Width) || float.IsNaN(size.Height))
                {
                    throw new InvalidOperationException("The implementing element returned NaN");
                }

                this.IsMeasureValid = true;
                this.DesiredSize = size;
            }
        }

        protected abstract void DrawImplementation();

        protected virtual Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }

        protected virtual Size MeasureOverride(Size availableSize)
        {
            return Size.Empty;
        }

        /// <summary>
        /// Defines the template for core-level arrange layout definition.
        /// </summary>
        /// <remarks>
        /// In WPF this method is defined on UIElement as protected virtual and has a base implementation.
        /// FrameworkElement (which derrives from UIElement) creates a sealed implemention, similar to the below,
        /// which discards UIElement's base implementation.
        /// </remarks>
        /// <param name="finalRect">The final area within the parent that element should use to arrange itself and its child elements.</param>
        private void ArrangeCore(Rect finalRect)
        {
        }

        /// <summary>
        /// Implements basic measure-pass layout system behavior.
        /// </summary>
        /// <remarks>
        /// In WPF this method is definded on UIElement as protected virtual and returns an empty Size.
        /// FrameworkElement (which derrives from UIElement) then creates a sealed implementation similar to the below.
        /// In XPF UIElement and FrameworkElement have been collapsed into a single class.
        /// </remarks>
        /// <param name="availableSize">The available size that the parent element can give to the child elements.</param>
        /// <returns>The desired size of this element in layout.</returns>
        private Size MeasureCore(Size availableSize)
        {
            Thickness margin = this.Margin;
            float horizontalMargin = margin.Left + margin.Right;
            float verticalMargin = margin.Top + margin.Bottom;

            Size availableSizeWithoutMargins = new Size((float)Math.Max((availableSize.Width - horizontalMargin), 0f), (float)Math.Max((availableSize.Height - verticalMargin), 0f));

            var size = this.MeasureOverride(availableSizeWithoutMargins);
            var width = size.Width + horizontalMargin;
            var height = size.Height + verticalMargin;

            if (width > availableSize.Width)
            {
                width = availableSize.Width;
            }

            if (height > availableSize.Height)
            {
                height = availableSize.Height;
            }

            return new Size(Math.Max(0.0f, width), Math.Max(0.0f, height));
        }
    }
}