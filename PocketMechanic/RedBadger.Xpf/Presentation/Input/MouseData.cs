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

        public override string ToString()
        {
            return string.Format("Action: {0}, Point: {1}", this.Action, this.Point);
        }
    }
}