namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Diagnostics;

    using RedBadger.Xpf.Internal;

    /// <summary>
    ///     A structure representing a Rectangle in 2D space
    /// </summary>
    [DebuggerDisplay("{Width} x {Height} @ {X}, {Y}")]
    public struct Rect : IEquatable<Rect>
    {
        /// <summary>
        ///     The Height of the <see cref = "Rect">Rect</see>.
        /// </summary>
        public double Height;

        /// <summary>
        ///     The Width of the <see cref = "Rect">Rect</see>.
        /// </summary>
        public double Width;

        /// <summary>
        ///     The X coordinate of the top left corner of the <see cref = "Rect">Rect</see>.
        /// </summary>
        public double X;

        /// <summary>
        ///     The Y coordinate of the top left corner of the <see cref = "Rect">Rect</see>.
        /// </summary>
        public double Y;

        private static readonly Rect empty = new Rect
            {
                X = double.PositiveInfinity, 
                Y = double.PositiveInfinity, 
                Width = double.NegativeInfinity, 
                Height = double.NegativeInfinity
            };

        /// <summary>
        ///     Constructs a new <see cref = "Rect">Rect</see> struct from a <see cref = "Size">Size</see>.
        /// </summary>
        /// <param name = "size">The <see cref = "Size">Size</see> of the <see cref = "Rect">Rect</see>.</param>
        public Rect(Size size)
            : this(new Point(), size)
        {
        }

        /// <summary>
        ///     Constructs a new Rect struct from a <see cref = "Point">Point</see> and a <see cref = "Size">Size</see>.
        /// </summary>
        /// <param name = "position">The position of the top left corner of the <see cref = "Rect">Rect</see>.</param>
        /// <param name = "size">The <see cref = "Size">Size</see> of the <see cref = "Rect">Rect</see>.</param>
        public Rect(Point position, Size size)
            : this(position.X, position.Y, size.Width, size.Height)
        {
        }

        /// <summary>
        ///     Constructs a new <see cref = "Rect">Rect</see> struct with the specified coordinates, width and height.
        /// </summary>
        /// <param name = "x">The x-coordinate of the top left corner of the <see cref = "Rect">Rect</see>.</param>
        /// <param name = "y">The y-coordinate of the top left corner of the <see cref = "Rect">Rect</see>.</param>
        /// <param name = "width">The width of the <see cref = "Rect">Rect</see>.</param>
        /// <param name = "height">The height of the <see cref = "Rect">Rect</see>.</param>
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

        /// <summary>
        ///     An empty <see cref = "Rect">Rect</see>.  Empty Rects have positive infinity coordinates and negative infinity size.
        /// </summary>
        public static Rect Empty
        {
            get
            {
                return empty;
            }
        }

        /// <summary>
        ///     Determines if the <see cref = "Rect">Rect</see> is empty - i.e. has positive infinity coordinates and negative infinity size.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.Width < 0d;
            }
        }

        /// <summary>
        ///     Gets the location of the top left corner of the <see cref = "Rect">Rect</see> in 2D space.
        /// </summary>
        public Point Location
        {
            get
            {
                return new Point(this.X, this.Y);
            }
        }

        /// <summary>
        ///     Gets the size of the <see cref = "Rect">Rect</see>.
        /// </summary>
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