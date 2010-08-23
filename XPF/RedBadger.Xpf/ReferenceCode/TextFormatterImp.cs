private TextLine FormatLineInternal(TextSource textSource, int firstCharIndex, int lineLength, double paragraphWidth, TextParagraphProperties paragraphProperties, TextLineBreak previousLineBreak, TextRunCache textRunCache)
{
    EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordText, EventTrace.Level.Verbose, EventTrace.Event.WClientStringBegin, "TextFormatterImp.FormatLineInternal Start");
    
    FormatSettings settings = this.PrepareFormatSettings(textSource, firstCharIndex, paragraphWidth, paragraphProperties, previousLineBreak, textRunCache, lineLength != 0, true, this._textFormattingMode);

    TextLine line = null;
    if ((!settings.Pap.AlwaysCollapsible && (previousLineBreak == null)) && (lineLength <= 0))
    {
        line = SimpleTextLine.Create(settings, firstCharIndex, RealToIdealFloor(paragraphWidth));
    }
    if (line == null)
    {
        line = new TextMetrics.FullTextLine(settings, firstCharIndex, lineLength, RealToIdealFloor(paragraphWidth), LineFlags.None);
    }

    EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordText, EventTrace.Level.Verbose, EventTrace.Event.WClientStringEnd, "TextFormatterImp.FormatLineInternal End");

    return line;
}