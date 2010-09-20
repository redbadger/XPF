namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{Width} x {Height}")]
    public struct Size : IEquatable<Size>
    {
        public double Height;

        public double Width;

        private static readonly Size empty = new Size
            {
               Width = double.NegativeInfinity, Height = double.NegativeInfinity 
            };

        public Size(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        public static Size Empty
        {
            get
            {
                return empty;
            }
        }

        /// <summary>
        /// Adds the Width and Height of a Size to those of another Size
        /// </summary>
        /// <param name="value1">The first Size</param>
        /// <param name="value2">The second Size</param>
        /// <returns>A Size whose Width and Height is the sum of the two Size structures supplied</returns>
        public static Size operator +(Size value1, Size value2)
        {
            return new Size(value1.Width + value2.Width, value1.Height + value2.Height);
        }

        /// <summary>
        /// Subtracts the Width and Height of a Size from those of another Size
        /// </summary>
        /// <param name="value1">The first Size</param>
        /// <param name="value2">The second Size</param>
        /// <returns>A Size whose Width and Height is the difference of the two Size structures supplied</returns>
        public static Size operator -(Size value1, Size value2)
        {
            return new Size(value1.Width - value2.Width, value1.Height - value2.Height);
        }

        public static bool operator ==(Size left, Size right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Size left, Size right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (obj.GetType() != typeof(Size))
            {
                return false;
            }

            return this.Equals((Size)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Width.GetHashCode() * 397) ^ this.Height.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("Width: {0}, Height: {1}", this.Width, this.Height);
        }

        public bool Equals(Size other)
        {
            return other.Width.Equals(this.Width) && other.Height.Equals(this.Height);
        }
    }
}