namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Diagnostics;

    using RedBadger.Xpf.Internal;

    [DebuggerDisplay("{Width} x {Height} @ {X}, {Y}")]
    public struct Rect : IEquatable<Rect>
    {
        public double Height;

        public double Width;

        public double X;

        public double Y;

        private static readonly Rect empty = new Rect
            {
                X = double.PositiveInfinity, 
                Y = double.PositiveInfinity, 
                Width = double.NegativeInfinity, 
                Height = double.NegativeInfinity
            };

        public Rect(Size size)
            : this(new Point(), size)
        {
        }

        public Rect(Point position, Size size)
            : this(position.X, position.Y, size.Width, size.Height)
        {
        }

        public Rect(double x, double y, double width, double height)
        {
            if (width < 0d || height < 0d)
            {
                throw new ArgumentException("width and height cannot be negative");
            }

            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public static Rect Empty
        {
            get
            {
                return empty;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this.Width < 0d;
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

        public static bool operator ==(Rect left, Rect right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rect left, Rect right)
        {
            return !left.Equals(right);
        }

        public bool Contains(Point point)
        {
            return this.Contains(point.X, point.Y);
        }

        public bool Contains(double x, double y)
        {
            if (this.IsEmpty)
            {
                return false;
            }

            return x >= this.X && x - this.Width <= this.X && y >= this.Y && y - this.Height <= this.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (obj.GetType() != typeof(Rect))
            {
                return false;
            }

            return this.Equals((Rect)obj);
        }

        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }

            unchecked
            {
                int result = this.Height.GetHashCode();
                result = (result * 397) ^ this.Width.GetHashCode();
                result = (result * 397) ^ this.X.GetHashCode();
                result = (result * 397) ^ this.Y.GetHashCode();
                return result;
            }
        }

        public bool Equals(Rect other)
        {
            if (other.IsEmpty)
            {
                return this.IsEmpty;
            }

            return other.Height.IsCloseTo(this.Height) && other.Width.IsCloseTo(this.Width) && other.X.IsCloseTo(this.X) &&
                   other.Y.IsCloseTo(this.Y);
        }
    }
}