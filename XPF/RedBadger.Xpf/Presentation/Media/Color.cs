namespace RedBadger.Xpf.Presentation.Media
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{R},{G},{B},{A}")]
    public struct Color : IEquatable<Color>
    {
        public byte A;

        public byte B;

        public byte G;

        public byte R;

        public Color(byte r, byte g, byte b, byte a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }

        public static Color FromUInt32(uint value)
        {
            return new Color
                {
                    A = (byte)((value & -16777216) >> 0x18), 
                    R = (byte)((value & 0xff0000) >> 0x10), 
                    G = (byte)((value & 0xff00) >> 8), 
                    B = (byte)(value & 0xff)
                };
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (obj.GetType() != typeof(Color))
            {
                return false;
            }

            return this.Equals((Color)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.A.GetHashCode();
                result = (result * 397) ^ this.B.GetHashCode();
                result = (result * 397) ^ this.G.GetHashCode();
                result = (result * 397) ^ this.R.GetHashCode();
                return result;
            }
        }

        public bool Equals(Color other)
        {
            return other.A == this.A && other.B == this.B && other.G == this.G && other.R == this.R;
        }
    }
}