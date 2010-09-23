namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Diagnostics;

    using RedBadger.Xpf.Internal;

    /// <summary>
    ///     A structure representing a Rectangle in 2D space
    /// </summary>
    [DebuggerDisplay("{X},{Y}: {Width}x{Height}")]
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
        ///     Initializes a new <see cref = "Rect">Rect</see> struct from a <see cref = "Size">Size</see>.
        /// </summary>
        /// <param name = "size">The <see cref = "Size">Size</see> of the <see cref = "Rect">Rect</see>.</param>
        public Rect(Size size)
            : this(new Point(), size)
        {
        }

        /// <summary>
        ///     Initializes a new <see cref = "Rect">Rect</see> struct from a <see cref = "Point">Point</see> and a <see cref = "Size">Size</see>.
        /// </summary>
        /// <param name = "position">The position of the top left corner of the <see cref = "Rect">Rect</see>.</param>
        /// <param name = "size">The <see cref = "Size">Size</see> of the <see cref = "Rect">Rect</see>.</param>
        public Rect(Point position, Size size)
            : this(position.X, position.Y, size.Width, size.Height)
        {
        }

        /// <summary>
        ///     Initializes a new <see cref = "Rect">Rect</see> struct with the specified coordinates, width and height.
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
        ///     Initializes a new <see cref = "Rect">Rect</see> that is just large enough to encompass the two specified <see cref = "Point">Point</see>s.
        /// </summary>
        /// <param name = "point1">The first <see cref = "Point">Point</see> to encompass.</param>
        /// <param name = "point2">The second <see cref = "Point">Point</see> to encompass.</param>
        public Rect(Point point1, Point point2)
        {
            this.X = Math.Min(point1.X, point2.X);
            this.Y = Math.Min(point1.Y, point2.Y);
            this.Width = (Math.Max(point1.X, point2.X) - this.X).EnsurePositive();
            this.Height = (Math.Max(point1.Y, point2.Y) - this.Y).EnsurePositive();
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
        ///     Gets the bottom side of the <see cref = "Rect">Rect</see>.
        /// </summary>
        public double Bottom
        {
            get
            {
                if (this.IsEmpty)
                {
                    return double.NegativeInfinity;
                }

                return this.Y + this.Height;
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
        ///     Gets the left side of the <see cref = "Rect">Rect</see>.
        /// </summary>
        public double Left
        {
            get
            {
                return this.X;
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
        ///     Gets the right side of the <see cref = "Rect">Rect</see>.
        /// </summary>
        public double Right
        {
            get
            {
                if (this.IsEmpty)
                {
                    return double.NegativeInfinity;
                }

                return this.X + this.Width;
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

        /// <summary>
        ///     Gets the top side of the <see cref = "Rect">Rect</see>.
        /// </summary>
        public double Top
        {
            get
            {
                return this.Y;
            }
        }

        /// <summary>
        ///     Compares two <see cref = "Rect">Rect</see>s for equality.
        /// </summary>
        /// <param name = "left">The left <see cref = "Rect">Rect</see>.</param>
        /// <param name = "right">The right <see cref = "Rect">Rect</see>.</param>
        /// <returns>true if the two <see cref = "Rect">Rect</see>s are equal.</returns>
        public static bool operator ==(Rect left, Rect right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Compares two <see cref = "Rect">Rect</see>s for inequality.
        /// </summary>
        /// <param name = "left">The left <see cref = "Rect">Rect</see>.</param>
        /// <param name = "right">The right <see cref = "Rect">Rect</see>.</param>
        /// <returns>true if the two <see cref = "Rect">Rect</see>s are not equal.</returns>
        public static bool operator !=(Rect left, Rect right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Tests if the <see cref = "Rect">Rect</see> contains the specified <see cref = "Point">Point</see>.
        /// </summary>
        /// <param name = "point">The <see cref = "Point">Point</see> to test.</param>
        /// <returns>True if the <see cref = "Rect">Rect</see> contains the specified <see cref = "Point">Point</see>.</returns>
        public bool Contains(Point point)
        {
            return this.Contains(point.X, point.Y);
        }

        /// <summary>
        ///     Tests if the <see cref = "Rect">Rect</see> contains the point described by the specified x and y coordinates.
        /// </summary>
        /// <param name = "x">The x coordinate of the point.</param>
        /// <param name = "y">The y coordinate of the point.</param>
        /// <returns>True if the <see cref = "Rect">Rect</see> contains the point described by the specified x and y coordinates.</returns>
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

        /// <summary>
        ///     Changes this <see cref = "Rect">Rect</see> so that it represents its intersection with the specified Rect.
        /// </summary>
        /// <param name = "rect">The <see cref = "Rect">Rect</see> to intersect with.</param>
        public void Intersect(Rect rect)
        {
            if (!this.IntersectsWith(rect))
            {
                this = empty;
            }
            else
            {
                double x = Math.Max(this.X, rect.X);
                double y = Math.Max(this.Y, rect.Y);
                this.Width = (Math.Min(this.X + this.Width, rect.X + rect.Width) - x).EnsurePositive();
                this.Height = (Math.Min(this.Y + this.Height, rect.Y + rect.Height) - y).EnsurePositive();
                this.X = x;
                this.Y = y;
            }
        }

        /// <summary>
        ///     Tests whether this <see cref = "Rect">Rect</see> instersects with the specified Rect.
        /// </summary>
        /// <param name = "rect">The <see cref = "Rect">Rect</see> to intersect with.</param>
        /// <returns>True if this <see cref = "Rect">Rect</see> instersects with the specified Rect.</returns>
        public bool IntersectsWith(Rect rect)
        {
            if (this.IsEmpty || rect.IsEmpty)
            {
                return false;
            }

            return (((rect.X <= (this.X + this.Width)) && ((rect.X + rect.Width) >= this.X)) &&
                    (rect.Y <= (this.Y + this.Height))) && ((rect.Y + rect.Height) >= this.Y);
        }

        public override string ToString()
        {
            return string.Format("{0},{1}: {2}x{3}", this.X, this.Y, this.Width, this.Height);
        }

        /// <summary>
        ///     Expands this <see cref = "Rect">Rect</see> to encompass the specified Rect.
        /// </summary>
        /// <param name = "rect">The <see cref = "Rect">Rect</see> to encompass.</param>
        public void Union(Rect rect)
        {
            if (this.IsEmpty)
            {
                this = rect;
            }
            else if (!rect.IsEmpty)
            {
                double x = Math.Min(this.Left, rect.Left);
                double y = Math.Min(this.Top, rect.Top);

                this.Width = rect.Width == double.PositiveInfinity || this.Width == double.PositiveInfinity
                                 ? double.PositiveInfinity
                                 : (Math.Max(this.Right, rect.Right) - x).EnsurePositive();

                this.Height = rect.Height == double.PositiveInfinity || this.Height == double.PositiveInfinity
                                  ? double.PositiveInfinity
                                  : (Math.Max(this.Bottom, rect.Bottom) - y).EnsurePositive();

                this.X = x;
                this.Y = y;
            }
        }

        /// <summary>
        ///     Expands this <see cref = "Rect">Rect</see> to encompass the specified <see cref = "Point">Point</see>.
        /// </summary>
        /// <param name = "point">The <see cref = "Point">Point</see> to include.</param>
        public void Union(Point point)
        {
            this.Union(new Rect(point, point));
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