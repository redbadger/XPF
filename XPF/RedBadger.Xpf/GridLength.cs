namespace RedBadger.Xpf
{
    using System;

    public struct GridLength
    {
        private static readonly GridLength auto = new GridLength(1, GridUnitType.Auto);

        private readonly GridUnitType gridUnitType;

        private readonly double value;

        public GridLength(double value)
            : this(value, GridUnitType.Pixel)
        {
        }

        public GridLength(double value, GridUnitType gridUnitType)
        {
            if (double.IsNaN(value))
            {
                throw new ArgumentException();
            }

            if (double.IsInfinity(value))
            {
                throw new ArgumentException();
            }

            this.value = gridUnitType == GridUnitType.Auto ? 1 : value;
            this.gridUnitType = gridUnitType;
        }

        /// <summary>
        ///     Gets a <see cref = "GridLength">GridLength</see> whose <see cref = "GridUnitType">GridUnitType</see> is set to <see cref = "RedBadger.Xpf.GridUnitType.Auto">GridUnitType.Auto</see>.
        /// </summary>
        public static GridLength Auto
        {
            get
            {
                return auto;
            }
        }

        public GridUnitType GridUnitType
        {
            get
            {
                return this.gridUnitType;
            }
        }

        public double Value
        {
            get
            {
                return this.value;
            }
        }
    }
}