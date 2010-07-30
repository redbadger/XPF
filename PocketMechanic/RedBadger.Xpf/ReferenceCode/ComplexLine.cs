internal void Format(int dcp, double width, TextParagraphProperties lineProperties, TextLineBreak textLineBreak, TextRunCache textRunCache, bool showParagraphEllipsis)
{
    this._mirror = lineProperties.FlowDirection == FlowDirection.RightToLeft;
    this._dcp = dcp;
    this._showParagraphEllipsis = showParagraphEllipsis;
    this._wrappingWidth = width;
    this._line = this._owner.TextFormatter.FormatLine(this, dcp, width, lineProperties, textLineBreak, textRunCache);
}

internal override void Arrange(VisualCollection vc, Vector lineOffset)
{
    int cp = base._dcp;
    IList<TextSpan<TextRun>> textRunSpans = base._line.GetTextRunSpans();
    double x = lineOffset.X;
    base.CalculateXOffsetShift();
    foreach (TextSpan<TextRun> span in textRunSpans)
    {
        TextRun run = span.Value;
        if (run is InlineObject)
        {
            FlowDirection direction;
            InlineObject obj2 = run as InlineObject;
            Visual parent = VisualTreeHelper.GetParent(obj2.Element) as Visual;
            if (parent != null)
            {
                ContainerVisual visual2 = parent as ContainerVisual;
                Invariant.Assert(visual2 != null, "parent should always derives from ContainerVisual");
                visual2.Children.Remove(obj2.Element);
            }
            Rect rect = base.GetBoundsFromPosition(cp, obj2.Length, out direction);
            ContainerVisual visualChild = new ContainerVisual();
            if (obj2.Element is FrameworkElement)
            {
                FlowDirection flowDirection = base._owner.FlowDirection;
                DependencyObject obj3 = ((FrameworkElement) obj2.Element).Parent;
                if (obj3 != null)
                {
                    flowDirection = (FlowDirection) obj3.GetValue(FrameworkElement.FlowDirectionProperty);
                }
                PtsHelper.UpdateMirroringTransform(base._owner.FlowDirection, flowDirection, visualChild, rect.Width);
            }
            vc.Add(visualChild);
            if (base._owner.UseLayoutRounding)
            {
                visualChild.Offset = new Vector(UIElement.RoundLayoutValue(lineOffset.X + rect.Left, FrameworkElement.DpiScaleX), UIElement.RoundLayoutValue(lineOffset.Y + rect.Top, FrameworkElement.DpiScaleY));
            }
            else
            {
                visualChild.Offset = new Vector(lineOffset.X + rect.Left, lineOffset.Y + rect.Top);
            }
            visualChild.Children.Add(obj2.Element);
            obj2.Element.Arrange(new Rect(obj2.Element.DesiredSize));
        }
        cp += span.Length;
    }
}

 

 
