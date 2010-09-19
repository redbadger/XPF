namespace RedBadger.Xpf.Presentation.Controls
{
    public class RowDefinition : DefinitionBase
    {
        public static readonly ReactiveProperty<GridLength> HeightProperty =
            ReactiveProperty<GridLength>.Register("Height", typeof(RowDefinition), new GridLength(1, GridUnitType.Star));

        public static readonly ReactiveProperty<double> MaxHeightProperty =
            ReactiveProperty<double>.Register("MaxHeight", typeof(RowDefinition), double.PositiveInfinity);

        public static readonly ReactiveProperty<double> MinHeightProperty =
            ReactiveProperty<double>.Register("MinHeight", typeof(RowDefinition), 0d);

        public RowDefinition()
            : base(DefinitionType.Row)
        {
        }

        public GridLength Height
        {
            get
            {
                return this.GetValue(HeightProperty);
            }

            set
            {
                this.SetValue(HeightProperty, value);
            }
        }

        public double MaxHeight
        {
            get
            {
                return this.GetValue(MaxHeightProperty);
            }

            set
            {
                this.SetValue(MaxHeightProperty, value);
            }
        }

        public double MinHeight
        {
            get
            {
                return this.GetValue(MinHeightProperty);
            }

            set
            {
                this.SetValue(MinHeightProperty, value);
            }
        }
    }
}