namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Diagnostics;

    /// <summary>
    ///     A struct representing a Vector with <see cref = "X">X</see> and <see cref = "Y">Y</see> components.
    /// </summary>
    [DebuggerDisplay("{X} x {Y}")]
    public struct Vector : IEquatable<Vector>
    {
        private const double RadiansToDegrees = 57.295779513082323;

        /// <summary>
        ///     The X component of the <see cref = "Vector">Vector</see>.
        /// </summary>
        public double X;

        /// <summary>
        ///     The Y component of the <see cref = "Vector">Vector</see>.
        /// </summary>
        public double Y;

        /// <summary>
        ///     Constructs a new <see cref = "Vector">Vector</see> structure.
        /// </summary>
        /// <param name = "x"> The <see cref = "X">X</see> component of the <see cref = "Vector">Vector</see>.</param>
        /// <param name = "y"> The <see cref = "Y">Y</see> component of the <see cref = "Vector">Vector</see>.</param>
        public Vector(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        ///     Gets a <see cref = "Vector">Vector</see> with <see cref = "X">X</see> and <see cref = "Y">Y</see> of zero.
        /// </summary>
        public static Vector Zero
        {
            get
            {
                return new Vector();
            }
        }

        /// <summary>
        ///     Gets the length of the <see cref = "Vector">Vector</see>.
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt(this.LengthSquared);
            }
        }

        /// <summary>
        ///     Gets the square of the <see cref = "Length">Length</see> of the <see cref = "Vector">Vector</see>.
        /// </summary>
        public double LengthSquared
        {
            get
            {
                return (this.X * this.X) + (this.Y * this.Y);
            }
        }

        /// <summary>
        ///     Adds a <see cref = "Vector">Vector</see> to another Vector.
        /// </summary>
        /// <param name = "vector1">The first <see cref = "Vector">Vector</see>.</param>
        /// <param name = "vector2">The second <see cref = "Vector">Vector</see>.</param>
        /// <returns>The sum of the two <see cref = "Vector">Vector</see>s.</returns>
        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        /// <summary>
        ///     Divides a <see cref = "Vector">Vector</see> by the specified <see cref = "scalar">scalar</see>.
        /// </summary>
        /// <param name = "vector">The <see cref = "Vector">Vector</see> to divide.</param>
        /// <param name = "scalar">The scalar by which to divide the <see cref = "Vector">Vector</see>.</param>
        /// <returns>A new <see cref = "Vector">Vector</see> whose <see cref = "X">X</see> and <see cref = "Y">Y</see> values have been divided by the specified <see cref = "scalar">scalar</see>.</returns>
        public static Vector operator /(Vector vector, double scalar)
        {
            return vector * (1d / scalar);
        }

        /// <summary>
        ///     Compares two <see cref = "Vector">Vector</see> structs for equality in their <see cref = "X">X</see> and <see cref = "Y">Y</see> components.
        /// </summary>
        /// <param name = "left">The first <see cref = "Vector">Vector</see> to compare.</param>
        /// <param name = "right">The second <see cref = "Vector">Vector</see> to compare.</param>
        /// <returns>Returns true if the two <see cref = "Vector">Vector</see> instances are equal.</returns>
        public static bool operator ==(Vector left, Vector right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Compares two <see cref = "Vector">Vector</see> structs for inequality in their <see cref = "X">X</see> and <see cref = "Y">Y</see> components.
        /// </summary>
        /// <param name = "left">The first <see cref = "Vector">Vector</see> to compare.</param>
        /// <param name = "right">The second <see cref = "Vector">Vector</see> to compare.</param>
        /// <returns>Returns true if the two <see cref = "Vector">Vector</see> instances are not equal.</returns>
        public static bool operator !=(Vector left, Vector right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Multiplies a <see cref = "Vector">Vector</see> by the specified <see cref = "scalar">scalar</see>.
        /// </summary>
        /// <param name = "vector">The <see cref = "Vector">Vector</see> to multiply.</param>
        /// <param name = "scalar">The scalar by which to multiply the <see cref = "Vector">Vector</see>.</param>
        /// <returns>A new <see cref = "Vector">Vector</see> whose <see cref = "X">X</see> and <see cref = "Y">Y</see> values have been multiplied by the specified <see cref = "scalar">scalar</see>.</returns>
        public static Vector operator *(Vector vector, double scalar)
        {
            return new Vector(vector.X * scalar, vector.Y * scalar);
        }

        /// <summary>
        ///     Calculates the angle in degrees between two <see cref = "Vector">Vector</see>s.
        /// </summary>
        /// <param name = "vector1">The first <see cref = "Vector">Vector</see>.</param>
        /// <param name = "vector2">The second <see cref = "Vector">Vector</see>.</param>
        /// <returns>The angle between the two specified <see cref = "Vector">Vector</see>s.</returns>
        public static double AngleBetween(Vector vector1, Vector vector2)
        {
            return Math.Atan2(CrossProduct(vector1, vector2), DotProduct(vector1, vector2)) * RadiansToDegrees;
        }

        /// <summary>
        ///     Calculates the Cross Product of two <see cref = "Vector">Vector</see>s.
        /// </summary>
        /// <param name = "vector1">The first <see cref = "Vector">Vector</see>.</param>
        /// <param name = "vector2">The second <see cref = "Vector">Vector</see>.</param>
        /// <returns>The Cross Product of the two <see cref = "Vector">Vector</see>s.</returns>
        public static double CrossProduct(Vector vector1, Vector vector2)
        {
            return (vector1.X * vector2.Y) - (vector1.Y * vector2.X);
        }

        /// <summary>
        ///     Calculates the Dot Product of two <see cref = "Vector">Vector</see>s.
        /// </summary>
        /// <param name = "vector1">The first <see cref = "Vector">Vector</see>.</param>
        /// <param name = "vector2">The second <see cref = "Vector">Vector</see>.</param>
        /// <returns>The Dot Product of the two <see cref = "Vector">Vector</see>s.</returns>
        public static double DotProduct(Vector vector1, Vector vector2)
        {
            return (vector1.X * vector2.X) + (vector1.Y * vector2.Y);
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

        /// <summary>
        ///     Normalizes the <see cref = "Vector">Vector</see> so that it is parallel but with unit length.
        /// </summary>
        public void Normalize()
        {
            this = this / Math.Max(Math.Abs(this.X), Math.Abs(this.Y));
            this = this / this.Length;
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