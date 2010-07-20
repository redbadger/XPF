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

 

 
