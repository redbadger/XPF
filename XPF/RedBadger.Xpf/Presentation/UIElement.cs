namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Presentation.Input;
    using RedBadger.Xpf.Presentation.Media;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    public abstract class UIElement : DependencyObject, IElement
    {
        public static readonly ReactiveProperty<object, UIElement> DataContextProperty =
            ReactiveProperty<object, UIElement>.Register("DataContext");

        public static readonly ReactiveProperty<double, UIElement> HeightProperty =
            ReactiveProperty<double, UIElement>.Register(
                "Height", double.NaN, ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<HorizontalAlignment, UIElement> HorizontalAlignmentProperty =
            ReactiveProperty<HorizontalAlignment, UIElement>.Register(
                "HorizontalAlignment", HorizontalAlignment.Stretch, ReactivePropertyChangedCallbacks.InvalidateArrange);

        public static readonly ReactiveProperty<bool, UIElement> IsMouseCapturedProperty =
            ReactiveProperty<bool, UIElement>.Register("IsMouseCaptured");

        public static readonly ReactiveProperty<Thickness, UIElement> MarginProperty =
            ReactiveProperty<Thickness, UIElement>.Register(
                "Margin", new Thickness(), ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<double, UIElement> MaxHeightProperty =
            ReactiveProperty<double, UIElement>.Register(
                "MaxHeight", double.PositiveInfinity, ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<double, UIElement> MaxWidthProperty =
            ReactiveProperty<double, UIElement>.Register(
                "MaxWidth", double.PositiveInfinity, ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<double, UIElement> MinHeightProperty =
            ReactiveProperty<double, UIElement>.Register(
                "MinHeight", ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<double, UIElement> MinWidthProperty =
            ReactiveProperty<double, UIElement>.Register("MinWidth", ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<VerticalAlignment, UIElement> VerticalAlignmentProperty =
            ReactiveProperty<VerticalAlignment, UIElement>.Register(
                "VerticalAlignment", VerticalAlignment.Stretch, ReactivePropertyChangedCallbacks.InvalidateArrange);

        public static readonly ReactiveProperty<double, UIElement> WidthProperty =
            ReactiveProperty<double, UIElement>.Register(
                "Width", double.NaN, ReactivePropertyChangedCallbacks.InvalidateMeasure);

        private readonly Subject<Gesture> gestures = new Subject<Gesture>();

        private Rect actualRect = Rect.Empty;

        private Size previousAvailableSize;

        private Rect previousFinalRect;

        protected UIElement()
        {
            this.Gestures.Subscribe(Observer.Create<Gesture>(this.OnNextGesture));
        }

        public double ActualHeight
        {
            get
            {
                return this.RenderSize.Height;
            }
        }

        public double ActualWidth
        {
            get
            {
                return this.RenderSize.Width;
            }
        }

        public object DataContext
        {
            get
            {
                return this.GetValue(DataContextProperty);
            }

            set
            {
                this.SetValue(DataContextProperty, value);
            }
        }

        public Size DesiredSize { get; private set; }

        public Subject<Gesture> Gestures
        {
            get
            {
                return this.gestures;
            }
        }

        public double Height
        {
            get
            {
                return this.GetValue(HeightProperty);
            }

            set
            {
                this.SetValue(HeightProperty, value);
            }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return this.GetValue(HorizontalAlignmentProperty);
            }

            set
            {
                this.SetValue(HorizontalAlignmentProperty, value);
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

        public bool IsMouseCaptured
        {
            get
            {
                return this.GetValue(IsMouseCapturedProperty);
            }

            private set
            {
                this.SetValue(IsMouseCapturedProperty, value);
            }
        }

        public Thickness Margin
        {
            get
            {
                return this.GetValue(MarginProperty);
            }

            set
            {
                this.SetValue(MarginProperty, value);
            }
        }

        public double MaxHeight
        {
            get
            {
                return this.GetValue(MaxHeightProperty);
            }

            set
            {
                this.SetValue(MaxHeightProperty, value);
            }
        }

        public double MaxWidth
        {
            get
            {
                return this.GetValue(MaxWidthProperty);
            }

            set
            {
                this.SetValue(MaxWidthProperty, value);
            }
        }

        public double MinHeight
        {
            get
            {
                return this.GetValue(MinHeightProperty);
            }

            set
            {
                this.SetValue(MinHeightProperty, value);
            }
        }

        public double MinWidth
        {
            get
            {
                return this.GetValue(MinWidthProperty);
            }

            set
            {
                this.SetValue(MinWidthProperty, value);
            }
        }

        public Size RenderSize { get; private set; }

        public VerticalAlignment VerticalAlignment
        {
            get
            {
                return this.GetValue(VerticalAlignmentProperty);
            }

            set
            {
                this.SetValue(VerticalAlignmentProperty, value);
            }
        }

        public IElement VisualParent { get; set; }

        public double Width
        {
            get
            {
                return this.GetValue(WidthProperty);
            }

            set
            {
                this.SetValue(WidthProperty, value);
            }
        }

        /// <remarks>
        ///     In WPF this is protected internal.  For the purposes of unit testing we've not made this protected.
        ///     TODO: implement a reflection based mechanism (for Moq?) to get back values from protected properties
        /// </remarks>
        internal Vector VisualOffset { get; set; }

        public bool CaptureMouse()
        {
            IRootElement rootElement;
            if (!this.IsMouseCaptured && this.TryGetRootElement(out rootElement))
            {
                this.IsMouseCaptured = rootElement.CaptureMouse(this);
            }

            return this.IsMouseCaptured;
        }

        public virtual void OnApplyTemplate()
        {
        }

        public void ReleaseMouseCapture()
        {
            IRootElement rootElement;
            if (this.IsMouseCaptured && this.TryGetRootElement(out rootElement))
            {
                rootElement.ReleaseMouseCapture(this);
                this.IsMouseCaptured = false;
            }
        }

        /// <summary>
        ///     Positions child elements and determines a size for a UIElement.
        ///     Parent elements call this method from their ArrangeOverride implementation to form a recursive layout update.
        ///     This method constitutes the second pass of a layout update.
        /// </summary>
        /// <param name = "finalRect">The final size that the parent computes for the child element, provided as a Rect instance.</param>
        public void Arrange(Rect finalRect)
        {
            if (double.IsNaN(finalRect.Width) || double.IsNaN(finalRect.Height))
            {
                throw new InvalidOperationException("Width and Height must be numbers");
            }

            if (double.IsPositiveInfinity(finalRect.Width) || double.IsPositiveInfinity(finalRect.Height))
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

        public Vector CalculateAbsoluteOffset()
        {
            Vector absoluteOffset = this.VisualOffset;

            if (this.VisualParent != null)
            {
                absoluteOffset += this.VisualParent.CalculateAbsoluteOffset();
            }

            this.actualRect = new Rect(absoluteOffset.X, absoluteOffset.Y, this.ActualWidth, this.ActualHeight);

            return absoluteOffset;
        }

        public virtual IEnumerable<IElement> GetVisualChildren()
        {
            yield break;
        }

        public bool HitTest(Point point)
        {
            if (this.actualRect != Rect.Empty)
            {
                return this.actualRect.Contains(point);
            }

            return false;
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
            if (double.IsNaN(availableSize.Width) || double.IsNaN(availableSize.Height))
            {
                throw new InvalidOperationException("AvailableSize Width or Height cannot be NaN");
            }

            if (!this.IsMeasureValid || availableSize.IsDifferentFrom(this.previousAvailableSize))
            {
                Size size = this.MeasureCore(availableSize);

                if (double.IsPositiveInfinity(size.Width) || double.IsPositiveInfinity(size.Height))
                {
                    throw new InvalidOperationException("The implementing element returned a PositiveInfinity");
                }

                if (double.IsNaN(size.Width) || double.IsNaN(size.Height))
                {
                    throw new InvalidOperationException("The implementing element returned NaN");
                }

                this.previousAvailableSize = availableSize;
                this.IsMeasureValid = true;
                this.DesiredSize = size;
            }
        }

        public bool TryGetRenderer(out IRenderer renderer)
        {
            IRootElement rootElement;
            if (this.TryGetRootElement(out rootElement))
            {
                renderer = rootElement.Renderer;
                return true;
            }

            renderer = null;
            return false;
        }

        public bool TryGetRootElement(out IRootElement rootElement)
        {
            var element = this as IRootElement;
            if (element != null)
            {
                rootElement = element;
                return true;
            }

            if (this.VisualParent != null)
            {
                return this.VisualParent.TryGetRootElement(out rootElement);
            }

            rootElement = null;
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
        ///     Returns the nearest ancestor of the specified type, which maybe itself or null.
        /// </summary>
        /// <typeparam name = "T">The <see cref = "Type">Type</see> of the ancestor</typeparam>
        /// <returns>The nearest ancestor of Type T</returns>
        protected override T GetNearestAncestorOfType<T>()
        {
            var ancestor = this as T;

            while (ancestor == null && this.VisualParent != null)
            {
                ancestor = this.VisualParent as T;
            }

            return ancestor;
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

        protected virtual void OnNextGesture(Gesture gesture)
        {
        }

        protected virtual void OnRender(IDrawingContext drawingContext)
        {
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
            Size finalSize = finalRect != Rect.Empty ? new Size(finalRect.Width, finalRect.Height) : new Size();

            Thickness margin = this.Margin;
            double horizontalMargin = margin.Left + margin.Right;
            double verticalMargin = margin.Top + margin.Bottom;

            finalSize.Width = Math.Max(0, finalSize.Width - horizontalMargin);
            finalSize.Height = Math.Max(0, finalSize.Height - verticalMargin);

            var desiredSizeWithoutMargins = new Size(
                Math.Max(0, this.DesiredSize.Width - horizontalMargin), 
                Math.Max(0, this.DesiredSize.Height - verticalMargin));

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

            double largestWidth = Math.Max(desiredSizeWithoutMargins.Width, minMax.MaxWidth);
            if (largestWidth.IsLessThan(finalSize.Width))
            {
                finalSize.Width = largestWidth;
            }

            double largestHeight = Math.Max(desiredSizeWithoutMargins.Height, minMax.MaxHeight);
            if (largestHeight.IsLessThan(finalSize.Height))
            {
                finalSize.Height = largestHeight;
            }

            Size renderSize = this.ArrangeOverride(finalSize);
            this.RenderSize = renderSize;

            var inkSize = new Size(
                Math.Min(renderSize.Width, minMax.MaxWidth), Math.Min(renderSize.Height, minMax.MaxHeight));
            var clientSize = new Size(
                Math.Max(0, finalRect.Width - horizontalMargin), Math.Max(0, finalRect.Height - verticalMargin));

            Vector offset = this.ComputeAlignmentOffset(clientSize, inkSize);
            offset.X += finalRect.X + margin.Left;
            offset.Y += finalRect.Y + margin.Top;

            this.VisualOffset = offset;
        }

        private Vector ComputeAlignmentOffset(Size clientSize, Size inkSize)
        {
            var vector = new Vector();
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
                    vector.X = (clientSize.Width - inkSize.Width) * 0.5;
                    break;
                case HorizontalAlignment.Left:
                    vector.X = 0;
                    break;
                case HorizontalAlignment.Right:
                    vector.X = clientSize.Width - inkSize.Width;
                    break;
            }

            switch (verticalAlignment)
            {
                case VerticalAlignment.Center:
                case VerticalAlignment.Stretch:
                    vector.Y = (clientSize.Height - inkSize.Height) * 0.5;
                    return vector;
                case VerticalAlignment.Bottom:
                    vector.Y = clientSize.Height - inkSize.Height;
                    return vector;
                case VerticalAlignment.Top:
                    vector.Y = 0;
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
            this.ResolveDeferredBindings(this.DataContext);
            this.OnApplyTemplate();

            Thickness margin = this.Margin;
            double horizontalMargin = margin.Left + margin.Right;
            double verticalMargin = margin.Top + margin.Bottom;

            var availableSizeWithoutMargins = new Size(
                Math.Max(availableSize.Width - horizontalMargin, 0), Math.Max(availableSize.Height - verticalMargin, 0));

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

            double desiredWidth = size.Width + horizontalMargin;
            double desiredHeight = size.Height + verticalMargin;

            if (desiredWidth > availableSize.Width)
            {
                desiredWidth = availableSize.Width;
            }

            if (desiredHeight > availableSize.Height)
            {
                desiredHeight = availableSize.Height;
            }

            return new Size(Math.Max(0, desiredWidth), Math.Max(0, desiredHeight));
        }
    }
}