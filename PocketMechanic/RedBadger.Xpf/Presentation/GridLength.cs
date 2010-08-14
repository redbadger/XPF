namespace RedBadger.Xpf.Presentation
{
    using System;

    public struct GridLength
    {
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