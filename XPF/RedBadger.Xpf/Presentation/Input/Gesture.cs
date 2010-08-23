namespace RedBadger.Xpf.Presentation.Input
{
    using System.Diagnostics;
    using System.Windows;

    using Vector = RedBadger.Xpf.Presentation.Vector;

    [DebuggerDisplay("{Type} @ P:{Point} D:{Delta}")]
    public struct Gesture
    {
        public GestureType Type;

        public Point Point;

        public Vector Delta;

        public Gesture(GestureType type, Point point) : this(type, point, Vector.Zero)
        {
        }

        public Gesture(GestureType type, Point point, Vector delta)
        {
            this.Type = type;
            this.Point = point;
            this.Delta = delta;
        }

        public override string ToString()
        {
            return string.Format("Type: {0}, Point: {1}, Delta: {2}", this.Type, this.Point, this.Delta);
        }
    }
}