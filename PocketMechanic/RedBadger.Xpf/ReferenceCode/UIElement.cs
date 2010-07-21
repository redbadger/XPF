public void Measure(Size availableSize)
{
    MeasureData measureData = this.MeasureData;

    if (DoubleUtil.IsNaN(availableSize.Width) || DoubleUtil.IsNaN(availableSize.Height))
    {
        throw new InvalidOperationException(SR.Get("UIElement_Layout_NaNMeasure"));
    }

    // Some Collapsed logic was here.

    if (!this.IsMeasureValid)
    {
        Size size2 = this._desiredSize;
        this.InvalidateArrange();

        Size size = new Size(0.0, 0.0);

        size = this.MeasureCore(availableSize);

        if (double.IsPositiveInfinity(size.Width) || double.IsPositiveInfinity(size.Height))
        {
            throw new InvalidOperationException(SR.Get("UIElement_Layout_PositiveInfinityReturned", new object[] { base.GetType().FullName }));
        }

        if (DoubleUtil.IsNaN(size.Width) || DoubleUtil.IsNaN(size.Height))
        {
            throw new InvalidOperationException(SR.Get("UIElement_Layout_NaNReturned", new object[] { base.GetType().FullName }));
        }

        this.MeasureDirty = false;

        this._desiredSize = size;
    }
}

public void Arrange(Rect finalRect)
{
    if ((double.IsPositiveInfinity(finalRect.Width) || double.IsPositiveInfinity(finalRect.Height)) || (DoubleUtil.IsNaN(finalRect.Width) || DoubleUtil.IsNaN(finalRect.Height)))
    {
        DependencyObject uIParent = this.GetUIParent() as UIElement;
        throw new InvalidOperationException(SR.Get("UIElement_Layout_InfinityArrange", new object[] { (uIParent == null) ? "" : uIParent.GetType().FullName, base.GetType().FullName }));
    }

    // Collapsed Logic Here
    else
    {
        if (!this.IsArrangeValid)
        {
            Size renderSize = this.RenderSize;
            bool flag3 = false;

            this.ArrangeCore(finalRect);
            
            this.ensureClip(finalRect.Size);
            flag3 = this.markForSizeChangedIfNeeded(renderSize, this.RenderSize);

            if (((flag3 || this.RenderingInvalidated) || neverArranged) && this.IsRenderable())
            {
                DrawingContext drawingContext = this.RenderOpen();
                try
                {
                    this.OnRender(drawingContext);
                }
                finally
                {
                    drawingContext.Close();
                    this.RenderingInvalidated = false;
                }
                this.updatePixelSnappingGuidelines();
            }
            if (neverArranged)
            {
                base.EndPropertyInitialization();
            }
        }
    }
}
 

 
