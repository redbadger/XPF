namespace RedBadger.Xpf.Presentation.Input
{
    using System.Diagnostics;
    using System.Windows;

    [DebuggerDisplay("{Action} @ {Point}")]
    public struct MouseData
    {
        public MouseAction Action;

        public Point Point;

        public MouseData(MouseAction action, Point point)
        {
            this.Action = action;
            this.Point = point;
        }
    }
}