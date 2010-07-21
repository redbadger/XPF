protected sealed override Size MeasureCore(Size availableSize)
{
    this.ApplyTemplate();

    Thickness margin = this.Margin;
    double num = margin.Left + margin.Right;
    double num2 = margin.Top + margin.Bottom;

    MeasureData measureData = base.MeasureData;

    Size transformSpaceBounds = new Size(Math.Max((double) (availableSize.Width - num), (double) 0.0), Math.Max((double) (availableSize.Height - num2), (double) 0.0));

    MinMax max = new MinMax(this);

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

protected sealed override void ArrangeCore(Rect finalRect)
{    
    Size untransformedDS;
    this.NeedsClipBounds = false;
    Size size = finalRect.Size;

    Thickness margin = this.Margin;
    double num = margin.Left + margin.Right;
    double num2 = margin.Top + margin.Bottom;
    size.Width = Math.Max((double) 0.0, (double) (size.Width - num));
    size.Height = Math.Max((double) 0.0, (double) (size.Height - num2));

    SizeBox box = UnclippedDesiredSizeField.GetValue(this);
    if (box == null)
    {
        untransformedDS = new Size(Math.Max((double) 0.0, (double) (base.DesiredSize.Width - num)), Math.Max((double) 0.0, (double) (base.DesiredSize.Height - num2)));
    }
    else
    {
        untransformedDS = new Size(box.Width, box.Height);
    }

    if (DoubleUtil.LessThan(size.Width, untransformedDS.Width))
    {
        this.NeedsClipBounds = true;
        size.Width = untransformedDS.Width;
    }
    if (DoubleUtil.LessThan(size.Height, untransformedDS.Height))
    {
        this.NeedsClipBounds = true;
        size.Height = untransformedDS.Height;
    }
    if (this.HorizontalAlignment != HorizontalAlignment.Stretch)
    {
        size.Width = untransformedDS.Width;
    }
    if (this.VerticalAlignment != VerticalAlignment.Stretch)
    {
        size.Height = untransformedDS.Height;
    }

    MinMax max = new MinMax(this);
    double num3 = Math.Max(untransformedDS.Width, max.maxWidth);
    if (DoubleUtil.LessThan(num3, size.Width))
    {
        this.NeedsClipBounds = true;
        size.Width = num3;
    }
    double num4 = Math.Max(untransformedDS.Height, max.maxHeight);
    if (DoubleUtil.LessThan(num4, size.Height))
    {
        this.NeedsClipBounds = true;
        size.Height = num4;
    }

    Size size8 = this.ArrangeOverride(size);
    base.RenderSize = size8;

    Size size9 = new Size(Math.Min(size8.Width, max.maxWidth), Math.Min(size8.Height, max.maxHeight));

    this.NeedsClipBounds |= DoubleUtil.LessThan(size9.Width, size8.Width) || DoubleUtil.LessThan(size9.Height, size8.Height);

    Size size10 = new Size(Math.Max((double) 0.0, (double) (finalRect.Width - num)), Math.Max((double) 0.0, (double) (finalRect.Height - num2)));

    this.NeedsClipBounds |= DoubleUtil.LessThan(size10.Width, size9.Width) || DoubleUtil.LessThan(size10.Height, size9.Height);

    Vector offset = this.ComputeAlignmentOffset(size10, size9);
    offset.X += finalRect.X + margin.Left;
    offset.Y += finalRect.Y + margin.Top;

    base.VisualOffset = offset;
}

private Vector ComputeAlignmentOffset(Size clientSize, Size inkSize)
{
    Vector vector = new Vector();
    HorizontalAlignment horizontalAlignment = this.HorizontalAlignment;
    VerticalAlignment verticalAlignment = this.VerticalAlignment;

    if ((horizontalAlignment == HorizontalAlignment.Stretch) && (inkSize.Width > clientSize.Width))
    {
        horizontalAlignment = HorizontalAlignment.Left;
    }
    if ((verticalAlignment == VerticalAlignment.Stretch) && (inkSize.Height > clientSize.Height))
    {
        verticalAlignment = VerticalAlignment.Top;
    }
    switch (horizontalAlignment)
    {
        case HorizontalAlignment.Center:
        case HorizontalAlignment.Stretch:
            vector.X = (clientSize.Width - inkSize.Width) * 0.5;
            break;

        default:
            if (horizontalAlignment == HorizontalAlignment.Right)
            {
                vector.X = clientSize.Width - inkSize.Width;
            }
            else
            {
                vector.X = 0.0;
            }
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
    }
    vector.Y = 0.0;
    return vector;
}