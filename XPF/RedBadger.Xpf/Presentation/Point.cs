namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{X},{Y}")]
    public struct Point : IEquatable<Point>
    {
        public double X;

        public double Y;

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector Zero
        {
            get
            {
                return new Vector();
            }
        }

        public static Vector operator +(Point value1, Point value2)
        {
            Vector newSize;
            newSize.X = value1.X + value2.X;
            newSize.Y = value1.Y + value2.Y;

            return newSize;
        }

        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (obj.GetType() != typeof(Vector))
            {
                return false;
            }

            return this.Equals((Vector)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.X.GetHashCode() * 397) ^ this.Y.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", this.X, this.Y);
        }

        public bool Equals(Point other)
        {
            return other.X.Equals(this.X) && other.Y.Equals(this.Y);
        }
    }
}