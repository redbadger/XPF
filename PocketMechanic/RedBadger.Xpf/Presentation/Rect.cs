namespace RedBadger.Xpf.Presentation
{
    using System.Diagnostics;

    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Internal;

    [DebuggerDisplay("{Width} x {Height} @ {X}, {Y}")]
    public struct Rect
    {
        public double Height;

        public double Width;

        public double X;

        public double Y;

        public Rect(Size size)
            : this(Vector.Zero, size)
        {
        }

        public Rect(Vector position, Size size)
            : this(position.X, position.Y, size.Width, size.Height)
        {
        }

        public Rect(double x, double y, double width, double height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public static Rect Empty
        {
            get
            {
                return new Rect();
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this.X.IsCloseTo(0) && this.Y.IsCloseTo(0) && this.Width.IsCloseTo(0) && this.Height.IsCloseTo(0);
            }
        }

        public Vector Position
        {
            get
            {
                return new Vector(this.X, this.Y);
            }
        }

        public Size Size
        {
            get
            {
                return new Size(this.Width, this.Height);
            }
        }
    }
}