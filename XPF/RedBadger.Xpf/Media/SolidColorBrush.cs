namespace RedBadger.Xpf.Media
{
    using System;

    /// <summary>
    ///     Represents a <see cref = "Brush">Brush</see> of the specified <see cref = "Color">Color</see> which can be used to paint an area with a solid color.
    /// </summary>
    public class SolidColorBrush : Brush, IConvertible
    {
        /// <summary>
        ///     <see cref = "ReactiveProperty{T}">ReactiveProperty</see> representing the <see cref = "Color">Color</see> property.
        /// </summary>
        public static readonly ReactiveProperty<Color> ColorProperty = ReactiveProperty<Color>.Register(
            "Color", typeof(SolidColorBrush), Colors.White);

        /// <summary>
        ///     Initializes a new instance of the <see cref = "SolidColorBrush">SolidColorBrush</see> class.
        /// </summary>
        /// <param name = "color">The <see cref = "Color">Color</see> with which to create this <see cref = "SolidColorBrush">SolidColorBrush</see>.</param>
        public SolidColorBrush(Color color)
        {
            this.Color = color;
        }

        /// <summary>
        ///     The <see cref = "Media.Color">Color</see> of the SolidColorBrush.
        /// </summary>
        public Color Color
        {
            get
            {
                return this.GetValue(ColorProperty);
            }

            set
            {
                this.SetValue(ColorProperty, value);
            }
        }

        public override string ToString()
        {
            return this.Color.ToString();
        }

        TypeCode IConvertible.GetTypeCode()
        {
            throw new InvalidCastException();
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return this.ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
    }
}