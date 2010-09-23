namespace RedBadger.Xpf
{
    using System;
    using System.Diagnostics;

    using RedBadger.Xpf.Internal;

    [DebuggerDisplay("{Left}, {Top}, {Right}, {Bottom}")]
    public struct Thickness : IEquatable<Thickness>
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

        /// <summary>
        ///     Adds two <see cref = "Thickness">Thickness</see> instances together (e.g. adds the two <see cref = "Left">Left</see> components, the two <see cref = "Top">Top</see> components etc).
        /// </summary>
        /// <param name = "left">The first <see cref = "Thickness">Thickness</see></param>
        /// <param name = "right">The second <see cref = "Thickness">Thickness</see></param>
        /// <returns>A <see cref = "Thickness">Thickness</see> whose components represent the sum of the two <see cref = "Thickness">Thickness</see> instances.</returns>
        public static Thickness operator +(Thickness left, Thickness right)
        {
            return new Thickness(
                left.Left + right.Left, left.Top + right.Top, left.Right + right.Right, left.Bottom + right.Bottom);
        }

        public static bool operator ==(Thickness left, Thickness right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Thickness left, Thickness right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (obj.GetType() != typeof(Thickness))
            {
                return false;
            }

            return this.Equals((Thickness)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.Bottom.GetHashCode();
                result = (result * 397) ^ this.Left.GetHashCode();
                result = (result * 397) ^ this.Right.GetHashCode();
                result = (result * 397) ^ this.Top.GetHashCode();
                return result;
            }
        }

        public bool Equals(Thickness other)
        {
            return other.Bottom.IsCloseTo(this.Bottom) && other.Left.IsCloseTo(this.Left) &&
                   other.Right.IsCloseTo(this.Right) && other.Top.IsCloseTo(this.Top);
        }
    }
}