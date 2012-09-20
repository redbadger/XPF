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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Subjects;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Input;
    using RedBadger.Xpf.Internal;

    public abstract class UIElement : ReactiveObject, IElement
    {
        public static readonly ReactiveProperty<object> DataContextProperty =
            ReactiveProperty<object>.Register("DataContext", typeof(UIElement), DataContextChanged);

        public static readonly ReactiveProperty<double> HeightProperty = ReactiveProperty<double>.Register(
            "Height", typeof(UIElement), double.NaN, ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<HorizontalAlignment> HorizontalAlignmentProperty =
            ReactiveProperty<HorizontalAlignment>.Register(
                "HorizontalAlignment", 
                typeof(UIElement), 
                HorizontalAlignment.Stretch, 
                ReactivePropertyChangedCallbacks.InvalidateArrange);

        public static readonly ReactiveProperty<bool> IsMouseCapturedProperty =
            ReactiveProperty<bool>.Register("IsMouseCaptured", typeof(UIElement));

        public static readonly ReactiveProperty<Thickness> MarginProperty =
            ReactiveProperty<Thickness>.Register(
                "Margin", typeof(UIElement), new Thickness(), ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<double> MaxHeightProperty =
            ReactiveProperty<double>.Register(
                "MaxHeight", 
                typeof(UIElement), 
                double.PositiveInfinity, 
                ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<double> MaxWidthProperty = ReactiveProperty<double>.Register(
            "MaxWidth", typeof(UIElement), double.PositiveInfinity, ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<double> MinHeightProperty =
            ReactiveProperty<double>.Register(
                "MinHeight", typeof(UIElement), ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<double> MinWidthProperty = ReactiveProperty<double>.Register(
            "MinWidth", typeof(UIElement), ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<VerticalAlignment> VerticalAlignmentProperty =
            ReactiveProperty<VerticalAlignment>.Register(
                "VerticalAlignment", 
                typeof(UIElement), 
                VerticalAlignment.Stretch, 
                ReactivePropertyChangedCallbacks.InvalidateArrange);

        public static readonly ReactiveProperty<double> WidthProperty = ReactiveProperty<double>.Register(
            "Width", typeof(UIElement), double.NaN, ReactivePropertyChangedCallbacks.InvalidateMeasure);

        private readonly Subject<Gesture> gestures = new Subject<Gesture>();

        private Rect clippingRect = Rect.Empty;

        private bool isClippingRequired;

        private Size previousAvailableSize;

        private Rect previousFinalRect;

        private Size unclippedSize;

        private Vector visualOffset;

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

        public Rect ClippingRect
        {
            get
            {
                return this.clippingRect;
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

        public Vector VisualOffset
        {
            get
            {
                return this.visualOffset;
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
                this.clippingRect = this.GetClippingRect(finalRect.Size);

                if (hasRenderer && drawingContext != null)
                {
                    this.OnRender(drawingContext);
                }

                this.previousFinalRect = finalRect;
                this.IsArrangeValid = true;
            }
        }

        public virtual IEnumerable<IElement> GetVisualChildren()
        {
            yield break;
        }

        public bool HitTest(Point point)
        {
            Vector absoluteOffset = Vector.Zero;
            IElement currentElement = this;

            while (currentElement != null)
            {
                absoluteOffset += currentElement.VisualOffset;
                currentElement = currentElement.VisualParent;
            }

            var hitTestRect = new Rect(absoluteOffset.X, absoluteOffset.Y, this.ActualWidth, this.ActualHeight);
            return hitTestRect.Contains(point);
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

        protected virtual Rect GetClippingRect(Size finalSize)
        {
            if (!this.isClippingRequired)
            {
                return Rect.Empty;
            }

            var max = new MinMax(this);
            Size renderSize = this.RenderSize;

            double maxWidth = double.IsPositiveInfinity(max.MaxWidth) ? renderSize.Width : max.MaxWidth;
            double maxHeight = double.IsPositiveInfinity(max.MaxHeight) ? renderSize.Height : max.MaxHeight;

            bool isClippingRequiredDueToMaxSize = maxWidth.IsLessThan(renderSize.Width) ||
                                                  maxHeight.IsLessThan(renderSize.Height);

            renderSize.Width = Math.Min(renderSize.Width, max.MaxWidth);
            renderSize.Height = Math.Min(renderSize.Height, max.MaxHeight);

            Thickness margin = this.Margin;
            double horizontalMargins = margin.Left + margin.Right;
            double verticalMargins = margin.Top + margin.Bottom;

            var clientSize = new Size(
                (finalSize.Width - horizontalMargins).EnsurePositive(), 
                (finalSize.Height - verticalMargins).EnsurePositive());

            bool isClippingRequiredDueToClientSize = clientSize.Width.IsLessThan(renderSize.Width) ||
                                                     clientSize.Height.IsLessThan(renderSize.Height);

            if (isClippingRequiredDueToMaxSize && !isClippingRequiredDueToClientSize)
            {
                return new Rect(0d, 0d, maxWidth, maxHeight);
            }

            if (!isClippingRequiredDueToClientSize)
            {
                return Rect.Empty;
            }

            Vector offset = this.ComputeAlignmentOffset(clientSize, renderSize);

            var clipRect = new Rect(-offset.X, -offset.Y, clientSize.Width, clientSize.Height);

            if (isClippingRequiredDueToMaxSize)
            {
                clipRect.Intersect(new Rect(0d, 0d, maxWidth, maxHeight));
            }

            return clipRect;
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

        private static void DataContextChanged(IReactiveObject source, ReactivePropertyChangeEventArgs<object> args)
        {
            ((UIElement)source).InvalidateMeasureOnDataContextInheritors();
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
            this.isClippingRequired = false;
            Size finalSize = finalRect != Rect.Empty ? new Size(finalRect.Width, finalRect.Height) : new Size();

            Thickness margin = this.Margin;

            finalSize = finalSize.Deflate(margin);

            Size unclippedDesiredSize = this.unclippedSize.IsEmpty
                                            ? this.DesiredSize.Deflate(margin)
                                            : this.unclippedSize;

            if (finalSize.Width.IsLessThan(unclippedDesiredSize.Width))
            {
                this.isClippingRequired = true;
                finalSize.Width = unclippedDesiredSize.Width;
            }

            if (finalSize.Height.IsLessThan(unclippedDesiredSize.Height))
            {
                this.isClippingRequired = true;
                finalSize.Height = unclippedDesiredSize.Height;
            }

            if (this.HorizontalAlignment != HorizontalAlignment.Stretch)
            {
                finalSize.Width = unclippedDesiredSize.Width;
            }

            if (this.VerticalAlignment != VerticalAlignment.Stretch)
            {
                finalSize.Height = unclippedDesiredSize.Height;
            }

            var minMax = new MinMax(this);

            double largestWidth = Math.Max(unclippedDesiredSize.Width, minMax.MaxWidth);
            if (largestWidth.IsLessThan(finalSize.Width))
            {
                finalSize.Width = largestWidth;
            }

            double largestHeight = Math.Max(unclippedDesiredSize.Height, minMax.MaxHeight);
            if (largestHeight.IsLessThan(finalSize.Height))
            {
                finalSize.Height = largestHeight;
            }

            Size renderSize = this.ArrangeOverride(finalSize);
            this.RenderSize = renderSize;

            var inkSize = new Size(
                Math.Min(renderSize.Width, minMax.MaxWidth), Math.Min(renderSize.Height, minMax.MaxHeight));

            this.isClippingRequired |= inkSize.Width.IsLessThan(renderSize.Width) ||
                                       inkSize.Height.IsLessThan(renderSize.Height);

            Size clientSize = finalRect.Size.Deflate(margin);

            this.isClippingRequired |= clientSize.Width.IsLessThan(inkSize.Width) ||
                                       clientSize.Height.IsLessThan(inkSize.Height);

            Vector offset = this.ComputeAlignmentOffset(clientSize, inkSize);
            offset.X += finalRect.X + margin.Left;
            offset.Y += finalRect.Y + margin.Top;

            this.visualOffset = offset;
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

        private object GetNearestDataContext()
        {
            IElement curentElement = this;
            object dataContext;

            do
            {
                dataContext = curentElement.DataContext;
                curentElement = curentElement.VisualParent;
            }
            while (dataContext == null && curentElement != null);

            return dataContext;
        }

        private void InvalidateMeasureOnDataContextInheritors()
        {
            IEnumerable<IElement> children = this.GetVisualChildren();
            if (children.Count() == 0)
            {
                this.InvalidateMeasure();
            }
            else
            {
                IEnumerable<UIElement> childrenInheritingDataContext =
                    children.OfType<UIElement>().Where(element => element.DataContext == null);

                foreach (UIElement element in childrenInheritingDataContext)
                {
                    element.InvalidateMeasureOnDataContextInheritors();
                }
            }
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
            this.ResolveDeferredBindings(this.GetNearestDataContext());
            this.OnApplyTemplate();

            Thickness margin = this.Margin;
            Size availableSizeWithoutMargins = availableSize.Deflate(margin);

            var minMax = new MinMax(this);

            availableSizeWithoutMargins.Width = availableSizeWithoutMargins.Width.Coerce(
                minMax.MinWidth, minMax.MaxWidth);
            availableSizeWithoutMargins.Height = availableSizeWithoutMargins.Height.Coerce(
                minMax.MinHeight, minMax.MaxHeight);

            Size size = this.MeasureOverride(availableSizeWithoutMargins);

            size = new Size(Math.Max(size.Width, minMax.MinWidth), Math.Max(size.Height, minMax.MinHeight));
            Size unclippedSize = size;

            bool isClippingRequired = false;
            if (size.Width > minMax.MaxWidth)
            {
                size.Width = minMax.MaxWidth;
                isClippingRequired = true;
            }

            if (size.Height > minMax.MaxHeight)
            {
                size.Height = minMax.MaxHeight;
                isClippingRequired = true;
            }

            Size desiredSizeWithMargins = size.Inflate(margin);

            if (desiredSizeWithMargins.Width > availableSize.Width)
            {
                desiredSizeWithMargins.Width = availableSize.Width;
                isClippingRequired = true;
            }

            if (desiredSizeWithMargins.Height > availableSize.Height)
            {
                desiredSizeWithMargins.Height = availableSize.Height;
                isClippingRequired = true;
            }

            this.unclippedSize = isClippingRequired ? unclippedSize : Size.Empty;

            return desiredSizeWithMargins;
        }
    }
}
