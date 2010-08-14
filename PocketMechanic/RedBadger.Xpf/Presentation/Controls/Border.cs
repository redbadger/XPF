namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Presentation.Media;

    using UIElement = RedBadger.Xpf.Presentation.UIElement;

    public class Border : UIElement
    {
        public static readonly XpfDependencyProperty BackgroundProperty = XpfDependencyProperty.Register(
            "Background", typeof(Brush), typeof(Border), new PropertyMetadata(null));

        public static readonly XpfDependencyProperty BorderBrushProperty = XpfDependencyProperty.Register(
            "BorderBrush", typeof(Brush), typeof(Border), new PropertyMetadata(null));

        public static readonly XpfDependencyProperty BorderThicknessProperty =
            XpfDependencyProperty.Register(
                "BorderThickness", 
                typeof(Thickness), 
                typeof(Border),
                new PropertyMetadata(new Thickness(), UIElementPropertyChangedCallbacks.PropertyOfTypeThickness));

        public static readonly XpfDependencyProperty ChildProperty = XpfDependencyProperty.Register(
            "Child", typeof(UIElement), typeof(Border), new PropertyMetadata(null, ChildPropertyChangedCallback));

        public static readonly XpfDependencyProperty PaddingProperty = XpfDependencyProperty.Register(
            "Padding", 
            typeof(Thickness), 
            typeof(Border),
            new PropertyMetadata(new Thickness(), UIElementPropertyChangedCallbacks.PropertyOfTypeThickness));

        private readonly IList<Rect> borders = new List<Rect>();

        private bool isBordersCollectionDirty;

        public Brush Background
        {
            get
            {
                return (Brush)this.GetValue(BackgroundProperty.Value);
            }

            set
            {
                this.SetValue(BackgroundProperty.Value, value);
            }
        }

        public Brush BorderBrush
        {
            get
            {
                return (Brush)this.GetValue(BorderBrushProperty.Value);
            }

            set
            {
                this.SetValue(BorderBrushProperty.Value, value);
            }
        }

        public Thickness BorderThickness
        {
            get
            {
                return (Thickness)this.GetValue(BorderThicknessProperty.Value);
            }

            set
            {
                this.SetValue(BorderThicknessProperty.Value, value);
            }
        }

        public UIElement Child
        {
            get
            {
                return (UIElement)this.GetValue(ChildProperty.Value);
            }

            set
            {
                this.SetValue(ChildProperty.Value, value);
            }
        }

        public Thickness Padding
        {
            get
            {
                return (Thickness)this.GetValue(PaddingProperty.Value);
            }

            set
            {
                this.SetValue(PaddingProperty.Value, value);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            UIElement child = this.Child;

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

            var borderSize = this.BorderThickness.Collapse();
            var paddingSize = this.Padding.Collapse();

            var borderThicknessAndPaddingSize = new Size(
                borderSize.Width + paddingSize.Width, borderSize.Height + paddingSize.Height);

            UIElement child = this.Child;
            if (child != null)
            {
                var childConstraint = new Size(
                    Math.Max(0, availableSize.Width - borderThicknessAndPaddingSize.Width), 
                    Math.Max(0, availableSize.Height - borderThicknessAndPaddingSize.Height));
                child.Measure(childConstraint);

                var size = new Size(
                    child.DesiredSize.Width + borderThicknessAndPaddingSize.Width, 
                    child.DesiredSize.Height + borderThicknessAndPaddingSize.Height);

                return size;
            }

            return borderThicknessAndPaddingSize;
        }

        protected override void OnRender(IDrawingContext drawingContext)
        {
            if (this.BorderThickness != new Thickness() && this.BorderBrush != null)
            {
                if (this.isBordersCollectionDirty)
                {
                    this.GenerateBorders();
                }

                foreach (var border in this.borders)
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
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var oldChild = args.OldValue as IElement;
            var newChild = args.NewValue as IElement;
            var border = (IElement)dependencyObject;

            border.InvalidateMeasure();

            if (oldChild != null)
            {
                oldChild.VisualParent = null;
            }

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