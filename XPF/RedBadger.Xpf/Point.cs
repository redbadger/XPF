#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

namespace RedBadger.Xpf
{
    using System;
    using System.Diagnostics;

    /// <summary>
    ///     A structrure with an <see cref = "X">X</see> and <see cref = "Y">Y</see> component representing a point in 2D space
    /// </summary>
    [DebuggerDisplay("{X},{Y}")]
    public struct Point : IEquatable<Point>
    {
        /// <summary>
        ///     The X component of the <see cref = "Point">Point</see>.
        /// </summary>
        public double X;

        /// <summary>
        ///     The Y component of the <see cref = "Point">Point</see>.
        /// </summary>
        public double Y;

        /// <summary>
        ///     Initializes a <see cref = "Point">Point</see> struct that represents a point in 2D space.
        /// </summary>
        /// <param name = "x">The <see cref = "X">X</see> component of the <see cref = "Point">Point</see>.</param>
        /// <param name = "y">The <see cref = "Y">Y</see> component of the <see cref = "Point">Point</see>.</param>
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        ///     Adds a <see cref = "Vector">Vector</see> to the <see cref = "Point">Point</see>.
        /// </summary>
        /// <param name = "point">The <see cref = "Point">Point</see> to which to add the <see cref = "Vector">Vector</see>.</param>
        /// <param name = "vector">The <see cref = "Vector">Vector</see> to add to the <see cref = "Point">Point</see>.</param>
        /// <returns>A new <see cref = "Point">Point</see> with the <see cref = "Vector">Vector</see> added.</returns>
        public static Point operator +(Point point, Vector vector)
        {
            return new Point(point.X + vector.X, point.Y + vector.Y);
        }

        /// <summary>
        ///     Compares two <see cref = "Point">Point</see>s for equality.
        /// </summary>
        /// <param name = "left">The left <see cref = "Point">Point</see>.</param>
        /// <param name = "right">The right <see cref = "Point">Point</see>.</param>
        /// <returns>true if the two <see cref = "Point">Point</see>s are equal.</returns>
        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Creates a new <see cref = "Size">Size</see> from the <see cref = "Point">Point</see>.
        /// </summary>
        /// <param name = "point">The <see cref = "Point">Point</see> to convert.</param>
        /// <returns>A new <see cref = "Size">Size</see> with equivalent dimensions.</returns>
        public static explicit operator Size(Point point)
        {
            return new Size(Math.Abs(point.X), Math.Abs(point.Y));
        }

        /// <summary>
        ///     Creates a new <see cref = "Vector">Vector</see> from the <see cref = "Point">Point</see>.
        /// </summary>
        /// <param name = "point">The <see cref = "Point">Point</see> to convert.</param>
        /// <returns>A new <see cref = "Vector">Vector</see> with equivalent dimensions.</returns>
        public static explicit operator Vector(Point point)
        {
            return new Vector(point.X, point.Y);
        }

        /// <summary>
        ///     Compares two <see cref = "Point">Point</see>s for inequality.
        /// </summary>
        /// <param name = "left">The left <see cref = "Point">Point</see>.</param>
        /// <param name = "right">The right <see cref = "Point">Point</see>.</param>
        /// <returns>true if the two <see cref = "Point">Point</see>s are not equal.</returns>
        public static bool operator !=(Point left, Point right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Subtracts a <see cref = "Vector">Vector</see> from the <see cref = "Point">Point</see>.
        /// </summary>
        /// <param name = "point">
        ///     The <see cref = "Point">Point</see> from which to subtract the <see cref = "Vector">Vector</see>.</param>
        /// <param name = "vector">
        ///     The <see cref = "Vector">Vector</see> to subtract from the <see cref = "Point">Point</see>.</param>
        /// <returns>A new <see cref = "Point">Point</see> with the <see cref = "Vector">Vector</see> subtracted.</returns>
        public static Point operator -(Point point, Vector vector)
        {
            return new Point(point.X - vector.X, point.Y - vector.Y);
        }

        /// <summary>
        ///     Subtracts a <see cref = "Point">Point</see> from another <see cref = "Point">Point</see>.
        /// </summary>
        /// <param name = "left">The left <see cref = "Point">Point</see> from which to subtract the right <see cref = "Point">Point</see>.</param>
        /// <param name = "right">The right <see cref = "Point">Point</see> to subtract from the left <see cref = "Point">Point</see>.</param>
        /// <returns>A new <see cref = "Vector">Vector</see> describing the difference between the two <see cref = "Point">Point</see>s.</returns>
        public static Vector operator -(Point left, Point right)
        {
            return new Vector(left.X - right.X, left.Y - right.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (obj.GetType() != typeof(Point))
            {
                return false;
            }

            return this.Equals((Point)obj);
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
