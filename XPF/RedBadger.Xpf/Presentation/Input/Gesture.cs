namespace RedBadger.Xpf.Presentation.Input
{
    using System.Diagnostics;

    [DebuggerDisplay("{Type} @ P:{Point} D:{Delta}")]
    public struct Gesture
    {
        public Vector Delta;

        public Point Point;

        public GestureType Type;

        public Gesture(GestureType type, Point point)
            : this(type, point, Vector.Zero)
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