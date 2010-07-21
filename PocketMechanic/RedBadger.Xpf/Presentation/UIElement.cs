namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Windows;

    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Internal;

    public abstract class UIElement : DependencyObject, IElement
    {
        public Size DesiredSize { get; private set; }

        public Vector2 DrawPosition { get; set; }

        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        ///   Gets a value indicating whether the computed size and position of child elements in this element's layout are valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the size and position of layout are valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsArrangeValid { get; private set; }

        public bool IsMeasureValid { get; private set; }

        public Thickness Margin { get; set; }

        public Size RenderSize { get; set; }

        public VerticalAlignment VerticalAlignment { get; set; }

        /// <remarks>
        ///   In WPF this is protected internal.  For the purposes of unit testing we've not made this protected.
        ///   TODO: implement a reflection based mechanism (for Moq?) to get back values from protected properties
        /// </remarks>
        internal Vector2 VisualOffset { get; set; }

        /// <summary>
        ///   Positions child elements and determines a size for a UIElement.
        ///   Parent elements call this method from their ArrangeOverride implementation to form a recursive layout update.
        ///   This method constitutes the second pass of a layout update.
        /// </summary>
        /// <param name = "finalRect">The final size that the parent computes for the child element, provided as a Rect instance.</param>
        public void Arrange(Rect finalRect)
        {
            if (float.IsNaN(finalRect.Width) || float.IsNaN(finalRect.Height))
            {
                throw new InvalidOperationException("Width and Height must be numbers");
            }

            if (float.IsPositiveInfinity(finalRect.Width) || float.IsPositiveInfinity(finalRect.Height))
            {
                throw new InvalidOperationException("Width and Height must be less than infinity");
            }

            if (!this.IsArrangeValid)
            {
                this.ArrangeCore(finalRect);
                this.IsArrangeValid = true;
            }
        }

        public void Draw()
        {
        }

        /// <summary>
        ///   Updates the DesiredSize of a UIElement.
        ///   Derrived elements call this method from their own MeasureOverride implementations to form a recursive layout update.
        ///   Calling this method constitutes the first pass (the "Measure" pass) of a layout update.
        /// </summary>
        /// <param name = "availableSize">
        ///   The available space that a parent element can allocate a child element.
        ///   A child element can request a larger space than what is available; the provided size might be accommodated.
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

        protected virtual Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }

        protected abstract void DrawImplementation();

        protected virtual Size MeasureOverride(Size availableSize)
        {
            return Size.Empty;
        }

        /// <summary>
        ///   Defines the template for core-level arrange layout definition.
        /// </summary>
        /// <remarks>
        ///   In WPF this method is defined on UIElement as protected virtual and has a base implementation.
        ///   FrameworkElement (which derrives from UIElement) creates a sealed implemention, similar to the below,
        ///   which discards UIElement's base implementation.
        /// </remarks>
        /// <param name = "finalRect">The final area within the parent that element should use to arrange itself and its child elements.</param>
        private void ArrangeCore(Rect finalRect)
        {
            Size size = finalRect.Size;

            Thickness margin = this.Margin;
            float horizontalMargin = margin.Left + margin.Right;
            float verticalMargin = margin.Top + margin.Bottom;

            size.Width = Math.Max(0f, size.Width - horizontalMargin);
            size.Height = Math.Max(0f, size.Height - verticalMargin);

            var desiredSizeWithoutMargins = new Size(
                Math.Max(0f, this.DesiredSize.Width - horizontalMargin),
                Math.Max(0f, this.DesiredSize.Height - verticalMargin));

            if (FloatUtil.LessThan(size.Width, desiredSizeWithoutMargins.Width))
            {
                size.Width = desiredSizeWithoutMargins.Width;
            }

            if (FloatUtil.LessThan(size.Height, desiredSizeWithoutMargins.Height))
            {
                size.Height = desiredSizeWithoutMargins.Height;
            }

            if (this.HorizontalAlignment != HorizontalAlignment.Stretch)
            {
                size.Width = desiredSizeWithoutMargins.Width;
            }

            if (this.VerticalAlignment != VerticalAlignment.Stretch)
            {
                size.Height = desiredSizeWithoutMargins.Height;
            }

            Size renderSize = this.ArrangeOverride(size);
            this.RenderSize = renderSize;

            var clientSize = new Size(Math.Max(0f, finalRect.Width - horizontalMargin), Math.Max(0f, finalRect.Height - verticalMargin));

            Vector2 offset = this.ComputeAlignmentOffset(clientSize, renderSize);
            offset.X += finalRect.X + margin.Left;
            offset.Y += finalRect.Y + margin.Top;

            this.VisualOffset = offset;
        }

        private Vector2 ComputeAlignmentOffset(Size clientSize, Size inkSize)
        {
            var vector = new Vector2();
            HorizontalAlignment horizontalAlignment = this.HorizontalAlignment;
            VerticalAlignment verticalAlignment = this.VerticalAlignment;

            if (horizontalAlignment == HorizontalAlignment.Stretch && inkSize.Width > clientSize.Width)
            {
                horizontalAlignment = HorizontalAlignment.Left;
            }

            if (verticalAlignment == VerticalAlignment.Stretch && inkSize.Height > clientSize.Height)
            {
                verticalAlignment = VerticalAlignment.Top;
            }

            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Center:
                case HorizontalAlignment.Stretch:
                    vector.X = (clientSize.Width - inkSize.Width) * 0.5f;
                    break;
                case HorizontalAlignment.Left:
                    vector.X = 0f;
                    break;
                case HorizontalAlignment.Right:
                    vector.X = clientSize.Width - inkSize.Width;
                    break;
            }

            switch (verticalAlignment)
            {
                case VerticalAlignment.Center:
                case VerticalAlignment.Stretch:
                    vector.Y = (clientSize.Height - inkSize.Height) * 0.5f;
                    return vector;
                case VerticalAlignment.Bottom:
                    vector.Y = clientSize.Height - inkSize.Height;
                    return vector;
                case VerticalAlignment.Top:
                    vector.Y = 0f;
                    break;
            }

            return vector;
        }

        /// <summary>
        ///   Implements basic measure-pass layout system behavior.
        /// </summary>
        /// <remarks>
        ///   In WPF this method is definded on UIElement as protected virtual and returns an empty Size.
        ///   FrameworkElement (which derrives from UIElement) then creates a sealed implementation similar to the below.
        ///   In XPF UIElement and FrameworkElement have been collapsed into a single class.
        /// </remarks>
        /// <param name = "availableSize">The available size that the parent element can give to the child elements.</param>
        /// <returns>The desired size of this element in layout.</returns>
        private Size MeasureCore(Size availableSize)
        {
            Thickness margin = this.Margin;
            float horizontalMargin = margin.Left + margin.Right;
            float verticalMargin = margin.Top + margin.Bottom;

            var availableSizeWithoutMargins = new Size(
                Math.Max((availableSize.Width - horizontalMargin), 0f),
                Math.Max((availableSize.Height - verticalMargin), 0f));

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