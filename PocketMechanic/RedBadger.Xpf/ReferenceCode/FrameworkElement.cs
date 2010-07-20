protected sealed override Size MeasureCore(Size availableSize)
{
    this.ApplyTemplate();

    Thickness margin = this.Margin;
    double num = margin.Left + margin.Right;
    double num2 = margin.Top + margin.Bottom;

    MeasureData measureData = base.MeasureData;

    Size transformSpaceBounds = new Size(Math.Max((double) (availableSize.Width - num), (double) 0.0), Math.Max((double) (availableSize.Height - num2), (double) 0.0));

    MinMax max = new MinMax(this);

    if (measureData != null)
    {
        measureData.AvailableSize = transformSpaceBounds;
    }

    Size size2 = this.MeasureOverride(transformSpaceBounds);

    if (measureData != null)
    {
        measureData.AvailableSize = availableSize;
    }

    size2 = new Size(Math.Max(size2.Width, max.minWidth), Math.Max(size2.Height, max.minHeight));
    Size size = size2;

    bool flag2 = false;
    if (size2.Width > max.maxWidth)
    {
        size2.Width = max.maxWidth;
        flag2 = true;
    }
    if (size2.Height > max.maxHeight)
    {
        size2.Height = max.maxHeight;
        flag2 = true;
    }

    double width = size2.Width + num;
    double height = size2.Height + num2;
    if (width > availableSize.Width)
    {
        width = availableSize.Width;
        flag2 = true;
    }
    if (height > availableSize.Height)
    {
        height = availableSize.Height;
        flag2 = true;
    }

    SizeBox box = UnclippedDesiredSizeField.GetValue(this);
    if ((flag2 || (width < 0.0)) || (height < 0.0))
    {
        if (box == null)
        {
            box = new SizeBox(size);
            UnclippedDesiredSizeField.SetValue(this, box);
        }
        else
        {
            box.Width = size.Width;
            box.Height = size.Height;
        }
    }
    else if (box != null)
    {
        UnclippedDesiredSizeField.ClearValue(this);
    }
    return new Size(Math.Max(0.0, width), Math.Max(0.0, height));
}

 

 
