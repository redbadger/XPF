namespace RedBadger.Xpf.Presentation
{
    using System.Diagnostics;

    using RedBadger.Xpf.Internal;

    [DebuggerDisplay("{Left}, {Top}, {Right}, {Bottom}")]
    public struct Thickness
    {
        public double Bottom;

        public double Left;

        public double Right;

        public double Top;

        public Thickness(double left, double top)
            : this(left, top, left, top)
        {
        }

        public Thickness(double uniformLength)
            : this(uniformLength, uniformLength, uniformLength, uniformLength)
        {
        }

        public Thickness(double left, double top, double right, double bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public static Thickness Empty
        {
            get
            {
                return new Thickness();
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this.Left.IsCloseTo(0) && this.Right.IsCloseTo(0) && this.Top.IsCloseTo(0) &&
                       this.Bottom.IsCloseTo(0);
            }
        }
    }
}