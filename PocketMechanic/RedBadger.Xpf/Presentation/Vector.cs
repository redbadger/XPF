namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Diagnostics;

    using RedBadger.Xpf.Internal;

    [DebuggerDisplay("{X} x {Y}")]
    public struct Vector : IEquatable<Vector>
    {
        public double X;

        public double Y;

        public Vector(double x, double y)
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

        public static Vector operator +(Vector value1, Vector value2)
        {
            Vector newSize;
            newSize.X = value1.X + value2.X;
            newSize.Y = value1.Y + value2.Y;

            return newSize;
        }

        public static bool operator ==(Vector left, Vector right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector left, Vector right)
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

        public bool Equals(Vector other)
        {
            return other.X.Equals(this.X) && other.Y.Equals(this.Y);
        }
    }
}