namespace RedBadger.Xpf.Controls
{
    using System.Collections.Generic;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Media;

    public class Border : UIElement
    {
        public static readonly ReactiveProperty<Brush> BackgroundProperty =
            ReactiveProperty<Brush>.Register("Background", typeof(Border));

        public static readonly ReactiveProperty<Brush> BorderBrushProperty =
            ReactiveProperty<Brush>.Register(
                "BorderBrush", typeof(Border), null, ReactivePropertyChangedCallbacks.InvalidateArrange);

        public static readonly ReactiveProperty<Thickness> BorderThicknessProperty =
            ReactiveProperty<Thickness>.Register(
                "BorderThickness", typeof(Border), new Thickness(), ReactivePropertyChangedCallbacks.InvalidateMeasure);

        public static readonly ReactiveProperty<IElement> ChildProperty = ReactiveProperty<IElement>.Register(
            "Child", typeof(Border), null, ChildPropertyChangedCallback);

        public static readonly ReactiveProperty<Thickness> PaddingProperty =
            ReactiveProperty<Thickness>.Register(
                "Padding", typeof(Border), new Thickness(), ReactivePropertyChangedCallbacks.InvalidateMeasure);

        private readonly IList<Rect> borders = new List<Rect>();

        private bool isBordersCollectionDirty;

        public Brush Background
        {
            get
            {
                return this.GetValue(BackgroundProperty);
            }

            set
            {
                this.SetValue(BackgroundProperty, value);
            }
        }

        public Brush BorderBrush
        {
            get
            {
                return this.GetValue(BorderBrushProperty);
            }

            set
            {
                this.SetValue(BorderBrushProperty, value);
            }
        }

        public Thickness BorderThickness
        {
            get
            {
                return this.GetValue(BorderThicknessProperty);
            }

            set
            {
                this.SetValue(BorderThicknessProperty, value);
            }
        }

        public IElement Child
        {
            get
            {
                return this.GetValue(ChildProperty);
            }

            set
            {
                this.SetValue(ChildProperty, value);
            }
        }

        public Thickness Padding
        {
            get
            {
                return this.GetValue(PaddingProperty);
            }

            set
            {
                this.SetValue(PaddingProperty, value);
            }
        }

        public override IEnumerable<IElement> GetVisualChildren()
        {
            IElement child = this.Child;
            if (child != null)
            {
                yield return child;
            }

            yield break;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            IElement child = this.Child;

            if (child != null)
            {
                var finalRect = new Rect(new Point(), finalSize);

                finalRect = finalRect.Deflate(this.BorderThickness);
                finalRect = finalRect.Deflate(this.Padding);
                child.Arrange(finalRect);
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.isBordersCollectionDirty = true;

            Thickness borderThicknessAndPadding = this.BorderThickness + this.Padding;

            IElement child = this.Child;
            if (child != null)
            {
                child.Measure(availableSize.Deflate(borderThicknessAndPadding));

                return child.DesiredSize.Inflate(borderThicknessAndPadding);
            }

            return borderThicknessAndPadding.Collapse();
        }

        protected override void OnRender(IDrawingContext drawingContext)
        {
            if (this.BorderThickness != new Thickness() && this.BorderBrush != null)
            {
                if (this.isBordersCollectionDirty)
                {
                    this.GenerateBorders();
                }

                foreach (Rect border in this.borders)
                {
                    drawingContext.DrawRectangle(border, this.BorderBrush);
                }
            }

            if (this.Background != null)
            {
                drawingContext.DrawRectangle(
                    new Rect(0, 0, this.ActualWidth, this.ActualHeight).Deflate(this.BorderThickness), this.Background);
            }
        }

        private static void ChildPropertyChangedCallback(
            IReactiveObject source, ReactivePropertyChangeEventArgs<IElement> change)
        {
            var border = (Border)source;
            border.InvalidateMeasure();

            IElement oldChild = change.OldValue;
            if (oldChild != null)
            {
                oldChild.VisualParent = null;
            }

            IElement newChild = change.NewValue;
            if (newChild != null)
            {
                newChild.VisualParent = border;
            }
        }

        private void GenerateBorders()
        {
            this.borders.Clear();

            if (this.BorderThickness.Left > 0)
            {
                this.borders.Add(new Rect(0, 0, this.BorderThickness.Left, this.ActualHeight));
            }

            if (this.BorderThickness.Top > 0)
            {
                this.borders.Add(
                    new Rect(
                        this.BorderThickness.Left, 
                        0, 
                        this.ActualWidth - this.BorderThickness.Left, 
                        this.BorderThickness.Top));
            }

            if (this.BorderThickness.Right > 0)
            {
                this.borders.Add(
                    new Rect(
                        this.ActualWidth - this.BorderThickness.Right, 
                        this.BorderThickness.Top, 
                        this.BorderThickness.Right, 
                        this.ActualHeight - this.BorderThickness.Top));
            }

            if (this.BorderThickness.Bottom > 0)
            {
                this.borders.Add(
                    new Rect(
                        this.BorderThickness.Left, 
                        this.ActualHeight - this.BorderThickness.Bottom, 
                        this.ActualWidth - (this.BorderThickness.Left + this.BorderThickness.Right), 
                        this.BorderThickness.Bottom));
            }

            this.isBordersCollectionDirty = false;
        }
    }
}