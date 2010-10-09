namespace RedBadger.Xpf.Media
{
    using System;
    using System.Diagnostics;

    /// <summary>
    ///     A struct representing a Color with Red (<see cref = "R">R</see>), Green (<see cref = "G">G</see>), Blue (<see cref = "B">B</see>) and Alpha (<see cref = "A">A</see>) channels.
    /// </summary>
    [DebuggerDisplay("{R},{G},{B},{A}")]
    public struct Color : IEquatable<Color>
    {
        /// <summary>
        ///     The Alpha channel of the <see cref = "Color">Color</see>
        /// </summary>
        public byte A;

        /// <summary>
        ///     The Blue channel of the <see cref = "Color">Color</see>
        /// </summary>
        public byte B;

        /// <summary>
        ///     The Green channel of the <see cref = "Color">Color</see>
        /// </summary>
        public byte G;

        /// <summary>
        ///     The Red channel of the <see cref = "Color">Color</see>
        /// </summary>
        public byte R;

        /// <summary>
        ///     Initializes a new instance of the <see cref = "Color">Color</see> struct.
        /// </summary>
        /// <param name = "r">The amount of Red in the <see cref = "Color">Color</see>.</param>
        /// <param name = "g">The amount of Green in the <see cref = "Color">Color</see>.</param>
        /// <param name = "b">The amount of Blue in the <see cref = "Color">Color</see>.</param>
        /// <param name = "a">The amount of Alpha in the <see cref = "Color">Color</see>.</param>
        public Color(byte r, byte g, byte b, byte a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        /// <summary>
        ///     Compares 2 Color instances for equality.
        /// </summary>
        /// <param name = "left">The first <see cref = "Color">Color</see> to compare.</param>
        /// <param name = "right">The second <see cref = "Color">Color</see> to compare.</param>
        /// <returns>True if both <see cref = "Color">Color</see>s have the same Red, Green, Blue and Alpha channels.</returns>
        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Compares 2 Color instances for inequality.
        /// </summary>
        /// <param name = "left">The first <see cref = "Color">Color</see> to compare.</param>
        /// <param name = "right">The second <see cref = "Color">Color</see> to compare.</param>
        /// <returns>True if the <see cref = "Color">Color</see>s have different Red, Green, Blue or Alpha channels.</returns>
        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Creates a <see cref = "Color">Color</see> instance from an unsigned integer representing the ARGB channels (in order of most-to-least significant bytes).
        /// </summary>
        /// <param name = "value"></param>
        /// <returns></returns>
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

        public override string ToString()
        {
            return string.Format("R: {0}, G: {1}, B: {2}, A: {3}", this.R, this.G, this.B, this.A);
        }

        public bool Equals(Color other)
        {
            return other.A == this.A && other.B == this.B && other.G == this.G && other.R == this.R;
        }
    }
}