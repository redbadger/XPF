namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Data;

    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Presentation.Media;

    public abstract class UIElement : DependencyObject, IElement
    {
        public static readonly XpfDependencyProperty HeightProperty = XpfDependencyProperty.Register(
            "Height", 
            typeof(float), 
            typeof(UIElement), 
            new PropertyMetadata(float.NaN, PropertyOfTypeFloatChangedCallback));

        public static readonly XpfDependencyProperty HorizontalAlignmentProperty =
            XpfDependencyProperty.Register(
                "HorizontalAlignment", 
                typeof(HorizontalAlignment), 
                typeof(UIElement), 
                new PropertyMetadata(HorizontalAlignment.Stretch, HorizontalAlignmentPropertyChangedCallback));

        public static readonly XpfDependencyProperty MarginProperty = XpfDependencyProperty.Register(
            "Margin", 
            typeof(Thickness), 
            typeof(UIElement), 
            new PropertyMetadata(Thickness.Empty, UIElementPropertyChangedCallbacks.PropertyOfTypeThickness));

        public static readonly XpfDependencyProperty MaxHeightProperty = XpfDependencyProperty.Register(
            "MaxHeight", 
            typeof(float), 
            typeof(UIElement), 
            new PropertyMetadata(float.PositiveInfinity, PropertyOfTypeFloatChangedCallback));

        public static readonly XpfDependencyProperty MaxWidthProperty = XpfDependencyProperty.Register(
            "MaxWidth", 
            typeof(float), 
            typeof(UIElement), 
            new PropertyMetadata(float.PositiveInfinity, PropertyOfTypeFloatChangedCallback));

        public static readonly XpfDependencyProperty MinHeightProperty = XpfDependencyProperty.Register(
            "MinHeight", typeof(float), typeof(UIElement), new PropertyMetadata(0f, PropertyOfTypeFloatChangedCallback));

        public static readonly XpfDependencyProperty MinWidthProperty = XpfDependencyProperty.Register(
            "MinWidth", typeof(float), typeof(UIElement), new PropertyMetadata(0f, PropertyOfTypeFloatChangedCallback));

        public static readonly XpfDependencyProperty VerticalAlignmentProperty =
            XpfDependencyProperty.Register(
                "VerticalAlignment", 
                typeof(VerticalAlignment), 
                typeof(UIElement), 
                new PropertyMetadata(VerticalAlignment.Stretch, VerticalAlignmentPropertyChangedCallback));

        public static readonly XpfDependencyProperty WidthProperty = XpfDependencyProperty.Register(
            "Width", 
            typeof(float), 
            typeof(UIElement), 
            new PropertyMetadata(float.NaN, PropertyOfTypeFloatChangedCallback));

        private readonly Dictionary<XpfDependencyProperty, BindingExpression> bindings =
            new Dictionary<XpfDependencyProperty, BindingExpression>();

        private Size previousAvailableSize;

        private Rect previousFinalRect;

        public Vector2 AbsoluteOffset
        {
            get
            {
                Vector2 absoluteOffset = this.VisualOffset;

                if (this.VisualParent != null)
                {
                    absoluteOffset += this.VisualParent.AbsoluteOffset;
                }

                return absoluteOffset;
            }
        }

        public float ActualHeight
        {
            get
            {
                return this.RenderSize.Height;
            }
        }

        public float ActualWidth
        {
            get
            {
                return this.RenderSize.Width;
            }
        }

        public Size DesiredSize { get; private set; }

        public float Height
        {
            get
            {
                return (float)this.GetValue(HeightProperty.Value);
            }

            set
            {
                this.SetValue(HeightProperty.Value, value);
            }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return (HorizontalAlignment)this.GetValue(HorizontalAlignmentProperty.Value);
            }

            set
            {
                this.SetValue(HorizontalAlignmentProperty.Value, value);
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the computed size and position of child elements in this element's layout are valid.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the size and position of layout are valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsArrangeValid { get; private set; }

        public bool IsMeasureValid { get; private set; }

        public Thickness Margin
        {
            get
            {
                return (Thickness)this.GetValue(MarginProperty.Value);
            }

            set
            {
                this.SetValue(MarginProperty.Value, value);
            }
        }

        public float MaxHeight
        {
            get
            {
                return (float)this.GetValue(MaxHeightProperty.Value);
            }

            set
            {
                this.SetValue(MaxHeightProperty.Value, value);
            }
        }

        public float MaxWidth
        {
            get
            {
                return (float)this.GetValue(MaxWidthProperty.Value);
            }

            set
            {
                this.SetValue(MaxWidthProperty.Value, value);
            }
        }

        public float MinHeight
        {
            get
            {
                return (float)this.GetValue(MinHeightProperty.Value);
            }

            set
            {
                this.SetValue(MinHeightProperty.Value, value);
            }
        }

        public float MinWidth
        {
            get
            {
                return (float)this.GetValue(MinWidthProperty.Value);
            }

            set
            {
                this.SetValue(MinWidthProperty.Value, value);
            }
        }

        public Size RenderSize { get; private set; }

        public VerticalAlignment VerticalAlignment
        {
            get
            {
                return (VerticalAlignment)this.GetValue(VerticalAlignmentProperty.Value);
            }

            set
            {
                this.SetValue(VerticalAlignmentProperty.Value, value);
            }
        }

        public IElement VisualParent { get; set; }

        public float Width
        {
            get
            {
                return (float)this.GetValue(WidthProperty.Value);
            }

            set
            {
                this.SetValue(WidthProperty.Value, value);
            }
        }

        /// <remarks>
        ///     In WPF this is protected internal.  For the purposes of unit testing we've not made this protected.
        ///     TODO: implement a reflection based mechanism (for Moq?) to get back values from protected properties
        /// </remarks>
        internal Vector2 VisualOffset { get; set; }

        /// <summary>
        ///     Positions child elements and determines a size for a UIElement.
        ///     Parent elements call this method from their ArrangeOverride implementation to form a recursive layout update.
        ///     This method constitutes the second pass of a layout update.
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

            if (!this.IsArrangeValid || finalRect.IsDifferentFrom(this.previousFinalRect))
            {
                IRenderer renderer;
                IDrawingContext drawingContext = null;

                bool hasRenderer = this.TryGetRenderer(out renderer);
                if (hasRenderer)
                {
                    drawingContext = renderer.GetDrawingContext(this);
                }

                this.ArrangeCore(finalRect);

                if (hasRenderer)
                {
                    this.OnRender(drawingContext);
                }

                this.previousFinalRect = finalRect;
                this.IsArrangeValid = true;
            }
        }

        public void ClearBinding(XpfDependencyProperty dependencyProperty)
        {
            BindingExpression bindingExpression;
            if (this.bindings.TryGetValue(dependencyProperty, out bindingExpression))
            {
                bindingExpression.ClearBinding();
                this.bindings.Remove(dependencyProperty);
            }
        }

        public void InvalidateArrange()
        {
            this.IsArrangeValid = false;

            IElement visualParent = this.VisualParent;
            if (visualParent != null)
            {
                visualParent.InvalidateArrange();
            }
        }

        public void InvalidateMeasure()
        {
            this.IsMeasureValid = false;

            IElement visualParent = this.VisualParent;
            if (visualParent != null)
            {
                visualParent.InvalidateMeasure();
            }

            this.InvalidateArrange();
        }

        /// <summary>
        ///     Updates the DesiredSize of a UIElement.
        ///     Derrived elements call this method from their own MeasureOverride implementations to form a recursive layout update.
        ///     Calling this method constitutes the first pass (the "Measure" pass) of a layout update.
        /// </summary>
        /// <param name = "availableSize">
        ///     The available space that a parent element can allocate a child element.
        ///     A child element can request a larger space than what is available; the provided size might be accommodated.
        /// </param>
        public void Measure(Size availableSize)
        {
            if (float.IsNaN(availableSize.Width) || float.IsNaN(availableSize.Height))
            {
                throw new InvalidOperationException("AvailableSize Width or Height cannot be NaN");
            }

            if (!this.IsMeasureValid || availableSize.IsDifferentFrom(this.previousAvailableSize))
            {
                Size size = this.MeasureCore(availableSize);

                if (float.IsPositiveInfinity(size.Width) || float.IsPositiveInfinity(size.Height))
                {
                    throw new InvalidOperationException("The implementing element returned a PositiveInfinity");
                }

                if (float.IsNaN(size.Width) || float.IsNaN(size.Height))
                {
                    throw new InvalidOperationException("The implementing element returned NaN");
                }

                this.previousAvailableSize = availableSize;
                this.IsMeasureValid = true;
                this.DesiredSize = size;
            }
        }

        public BindingExpression SetBinding(XpfDependencyProperty dependencyProperty, Binding binding)
        {
            BindingExpression bindingExpression;
            if (!this.bindings.TryGetValue(dependencyProperty, out bindingExpression))
            {
                bindingExpression = new BindingExpression(this, dependencyProperty);
                this.bindings.Add(dependencyProperty, bindingExpression);
            }

            bindingExpression.SetBinding(binding);
            return bindingExpression;
        }

        public bool TryGetRenderer(out IRenderer renderer)
        {
            var rootElement = this as IRootElement;
            if (rootElement != null)
            {
                renderer = rootElement.Renderer;
                return true;
            }

            if (this.VisualParent != null)
            {
                return this.VisualParent.TryGetRenderer(out renderer);
            }

            renderer = null;
            return false;
        }

        /// <summary>
        ///     When overridden in a derived class, positions child elements and determines a size for a UIElement derived class.
        /// </summary>
        /// <param name = "finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected virtual Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }

        /// <summary>
        ///     When overridden in a derived class, measures the size in layout required for child elements and determines a size for the UIElement-derived class.
        /// </summary>
        /// <param name = "availableSize">
        ///     The available size that this element can give to child elements.
        ///     Infinity can be specified as a value to indicate that the element will size to whatever content is available.
        /// </param>
        /// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
        protected virtual Size MeasureOverride(Size availableSize)
        {
            return Size.Empty;
        }

        protected virtual void OnRender(IDrawingContext drawingContext)
        {
        }

        private static void HorizontalAlignmentPropertyChangedCallback(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var newValue = (HorizontalAlignment)args.NewValue;
            var oldValue = (HorizontalAlignment)args.OldValue;

            if (newValue != oldValue)
            {
                var uiElement = dependencyObject as UIElement;
                if (uiElement != null)
                {
                    uiElement.InvalidateArrange();
                }
            }
        }

        private static void PropertyOfTypeFloatChangedCallback(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var newValue = (float)args.NewValue;
            var oldValue = (float)args.OldValue;

            if (newValue.IsDifferentFrom(oldValue))
            {
                var uiElement = dependencyObject as UIElement;
                if (uiElement != null)
                {
                    uiElement.InvalidateMeasure();
                }
            }
        }

        private static void VerticalAlignmentPropertyChangedCallback(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var newValue = (VerticalAlignment)args.NewValue;
            var oldValue = (VerticalAlignment)args.OldValue;

            if (newValue != oldValue)
            {
                var uiElement = dependencyObject as UIElement;
                if (uiElement != null)
                {
                    uiElement.InvalidateArrange();
                }
            }
        }

        /// <summary>
        ///     Defines the template for core-level arrange layout definition.
        /// </summary>
        /// <remarks>
        ///     In WPF this method is defined on UIElement as protected virtual and has a base implementation.
        ///     FrameworkElement (which derrives from UIElement) creates a sealed implemention, similar to the below,
        ///     which discards UIElement's base implementation.
        /// </remarks>
        /// <param name = "finalRect">The final area within the parent that element should use to arrange itself and its child elements.</param>
        private void ArrangeCore(Rect finalRect)
        {
            Size finalSize = finalRect.Size;

            Thickness margin = this.Margin;
            float horizontalMargin = margin.Left + margin.Right;
            float verticalMargin = margin.Top + margin.Bottom;

            finalSize.Width = Math.Max(0f, finalSize.Width - horizontalMargin);
            finalSize.Height = Math.Max(0f, finalSize.Height - verticalMargin);

            var desiredSizeWithoutMargins = new Size(
                Math.Max(0f, this.DesiredSize.Width - horizontalMargin), 
                Math.Max(0f, this.DesiredSize.Height - verticalMargin));

            if (finalSize.Width.IsLessThan(desiredSizeWithoutMargins.Width))
            {
                finalSize.Width = desiredSizeWithoutMargins.Width;
            }

            if (finalSize.Height.IsLessThan(desiredSizeWithoutMargins.Height))
            {
                finalSize.Height = desiredSizeWithoutMargins.Height;
            }

            if (this.HorizontalAlignment != HorizontalAlignment.Stretch)
            {
                finalSize.Width = desiredSizeWithoutMargins.Width;
            }

            if (this.VerticalAlignment != VerticalAlignment.Stretch)
            {
                finalSize.Height = desiredSizeWithoutMargins.Height;
            }

            var minMax = new MinMax(this);

            float largestWidth = Math.Max(desiredSizeWithoutMargins.Width, minMax.MaxWidth);
            if (largestWidth.IsLessThan(finalSize.Width))
            {
                finalSize.Width = largestWidth;
            }

            float largestHeight = Math.Max(desiredSizeWithoutMargins.Height, minMax.MaxHeight);
            if (largestHeight.IsLessThan(finalSize.Height))
            {
                finalSize.Height = largestHeight;
            }

            Size renderSize = this.ArrangeOverride(finalSize);
            this.RenderSize = renderSize;

            var inkSize = new Size(
                Math.Min(renderSize.Width, minMax.MaxWidth), Math.Min(renderSize.Height, minMax.MaxHeight));
            var clientSize = new Size(
                Math.Max(0f, finalRect.Width - horizontalMargin), Math.Max(0f, finalRect.Height - verticalMargin));

            Vector2 offset = this.ComputeAlignmentOffset(clientSize, inkSize);
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
        ///     Implements basic measure-pass layout system behavior.
        /// </summary>
        /// <remarks>
        ///     In WPF this method is definded on UIElement as protected virtual and returns an empty Size.
        ///     FrameworkElement (which derrives from UIElement) then creates a sealed implementation similar to the below.
        ///     In XPF UIElement and FrameworkElement have been collapsed into a single class.
        /// </remarks>
        /// <param name = "availableSize">The available size that the parent element can give to the child elements.</param>
        /// <returns>The desired size of this element in layout.</returns>
        private Size MeasureCore(Size availableSize)
        {
            Thickness margin = this.Margin;
            float horizontalMargin = margin.Left + margin.Right;
            float verticalMargin = margin.Top + margin.Bottom;

            var availableSizeWithoutMargins = new Size(
                Math.Max(availableSize.Width - horizontalMargin, 0f), 
                Math.Max(availableSize.Height - verticalMargin, 0f));

            var minMax = new MinMax(this);

            Size size = this.MeasureOverride(availableSizeWithoutMargins);

            size = new Size(Math.Max(size.Width, minMax.MinWidth), Math.Max(size.Height, minMax.MinHeight));

            if (size.Width > minMax.MaxWidth)
            {
                size.Width = minMax.MaxWidth;
            }

            if (size.Height > minMax.MaxHeight)
            {
                size.Height = minMax.MaxHeight;
            }

            float desiredWidth = size.Width + horizontalMargin;
            float desiredHeight = size.Height + verticalMargin;

            if (desiredWidth > availableSize.Width)
            {
                desiredWidth = availableSize.Width;
            }

            if (desiredHeight > availableSize.Height)
            {
                desiredHeight = availableSize.Height;
            }

            return new Size(Math.Max(0.0f, desiredWidth), Math.Max(0.0f, desiredHeight));
        }
    }
}