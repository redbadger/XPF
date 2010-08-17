namespace RedBadger.Xpf.Presentation.Input
{
    using System.Windows;

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