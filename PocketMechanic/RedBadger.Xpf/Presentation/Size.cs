namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Diagnostics;

    using RedBadger.Xpf.Internal;

    [DebuggerDisplay("{Width} x {Height}")]
    public struct Size : IEquatable<Size>
    {
        public double Height;

        public double Width;

        public Size(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        public static Size Empty
        {
            get
            {
                return new Size();
            }
        }

        public static Size operator +(Size value1, Size value2)
        {
            Size newSize;
            newSize.Width = value1.Width + value2.Width;
            newSize.Height = value1.Height + value2.Height;

            return newSize;
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