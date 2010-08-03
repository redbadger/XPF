namespace RedBadger.Xpf.Presentation
{
    using System;

    public struct GridLength
    {
        private readonly GridUnitType gridUnitType;

        private readonly float value;

        public GridLength(float value)
            : this(value, GridUnitType.Pixel)
        {
        }

        public GridLength(float value, GridUnitType gridUnitType)
        {
            if (float.IsNaN(value))
            {
                throw new ArgumentException();
            }

            if (float.IsInfinity(value))
            {
                throw new ArgumentException();
            }

            this.value = gridUnitType == GridUnitType.Auto ? 1f : value;
            this.gridUnitType = gridUnitType;
        }

        public GridUnitType GridUnitType
        {
            get
            {
                return this.gridUnitType;
            }
        }

        public float Value
        {
            get
            {
                return this.value;
            }
        }
    }
}