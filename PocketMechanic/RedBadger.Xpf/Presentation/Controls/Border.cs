namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Presentation.Media;

    using Rect = RedBadger.Xpf.Presentation.Rect;
    using Size = RedBadger.Xpf.Presentation.Size;
    using Thickness = RedBadger.Xpf.Presentation.Thickness;
    using UIElement = RedBadger.Xpf.Presentation.UIElement;

    public class Border : UIElement
    {
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background", typeof(Brush), typeof(Border), new PropertyMetadata(null));

        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            "BorderBrush", typeof(Brush), typeof(Border), new PropertyMetadata(null));

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register(
                "BorderThickness", 
                typeof(Thickness), 
                typeof(Border), 
                new PropertyMetadata(Thickness.Empty, UIElementPropertyChangedCallbacks.PropertyOfTypeThickness));

        public static readonly DependencyProperty ChildProperty = DependencyProperty.Register(
            "Child", typeof(UIElement), typeof(Border), new PropertyMetadata(null, ChildPropertyChangedCallback));

        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            "Padding", 
            typeof(Thickness), 
            typeof(Border), 
            new PropertyMetadata(Thickness.Empty, UIElementPropertyChangedCallbacks.PropertyOfTypeThickness));

        private readonly IList<Rectangle> borders = new List<Rectangle>();

        private readonly IPrimitivesService primitivesService;

        private bool isBordersCollectionDirty;

        public Border(IPrimitivesService primitivesService)
        {
            this.primitivesService = primitivesService;
        }

        public Brush Background
        {
            get
            {
                return (Brush)this.GetValue(BackgroundProperty);
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
                return (Brush)this.GetValue(BorderBrushProperty);
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
                return (Thickness)this.GetValue(BorderThicknessProperty);
            }

            set
            {
                this.SetValue(BorderThicknessProperty, value);
            }
        }

        public UIElement Child
        {
            get
            {
                return (UIElement)this.GetValue(ChildProperty);
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
                return (Thickness)this.GetValue(PaddingProperty);
            }

            set
            {
                this.SetValue(PaddingProperty, value);
            }
        }

        public override void Render(ISpriteBatch spriteBatch)
        {
            if (this.Child != null)
            {
                this.Child.Render(spriteBatch);
            }

            var solidColorBorderBrush = this.BorderBrush as SolidColorBrush;
            if (solidColorBorderBrush != null)
            {
                if (this.isBordersCollectionDirty)
                {
                    this.GenerateBorders();
                }

                foreach (var border in this.borders)
                {
                    spriteBatch.Draw(this.primitivesService.SinglePixel, border, solidColorBorderBrush.Color);
                }
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            UIElement child = this.Child;

            if (child != null)
            {
                var finalRect = new Rect(finalSize);

                finalRect = finalRect.Defalte(this.BorderThickness);
                finalRect = finalRect.Defalte(this.Padding);
                child.Arrange(finalRect);
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.isBordersCollectionDirty = true;

            UIElement child = this.Child;
            Size borderThicknessAndPaddingSize = this.BorderThickness.Collapse() + this.Padding.Collapse();

            if (child != null)
            {
                var childConstraint = new Size(
                    Math.Max(0f, availableSize.Width - borderThicknessAndPaddingSize.Width), 
                    Math.Max(0f, availableSize.Height - borderThicknessAndPaddingSize.Height));
                child.Measure(childConstraint);

                var size = new Size(
                    child.DesiredSize.Width + borderThicknessAndPaddingSize.Width, 
                    child.DesiredSize.Height + borderThicknessAndPaddingSize.Height);

                return size;
            }

            return borderThicknessAndPaddingSize;
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

            var leftBorderThickness = (int)this.BorderThickness.Left;
            var topBorderThickness = (int)this.BorderThickness.Top;
            var rightBorderThickness = (int)this.BorderThickness.Right;
            var bottomBorderThickness = (int)this.BorderThickness.Bottom;
            var actualHeight = (int)this.ActualHeight;
            var actualWidth = (int)this.ActualWidth;
            var visualOffsetX = (int)this.VisualOffset.X;
            var visualOffsetY = (int)this.VisualOffset.Y;

            if (leftBorderThickness > 0)
            {
                var leftBorder = new Rectangle(0, 0, leftBorderThickness, actualHeight);
                leftBorder.X += visualOffsetX;
                leftBorder.Y += visualOffsetY;

                this.borders.Add(leftBorder);
            }

            if (topBorderThickness > 0)
            {
                var topBorder = new Rectangle(
                    leftBorderThickness, 0, actualWidth - leftBorderThickness, topBorderThickness);
                topBorder.X += visualOffsetX;
                topBorder.Y += visualOffsetY;

                this.borders.Add(topBorder);
            }

            if (rightBorderThickness > 0)
            {
                var rightBorder = new Rectangle(
                    actualWidth - rightBorderThickness, 
                    topBorderThickness, 
                    rightBorderThickness, 
                    actualHeight - topBorderThickness);
                rightBorder.X += visualOffsetX;
                rightBorder.Y += visualOffsetY;

                this.borders.Add(rightBorder);
            }

            if (bottomBorderThickness > 0)
            {
                var bottomBorder = new Rectangle(
                    leftBorderThickness, 
                    actualHeight - bottomBorderThickness, 
                    actualWidth - (leftBorderThickness + rightBorderThickness), 
                    bottomBorderThickness);
                bottomBorder.X += visualOffsetX;
                bottomBorder.Y += visualOffsetY;
                this.borders.Add(bottomBorder);
            }

            this.isBordersCollectionDirty = false;
        }
    }
}