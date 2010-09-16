namespace RedBadger.Xpf.Presentation.Controls
{
    public class RowDefinition : DefinitionBase
    {
        public static readonly ReactiveProperty<GridLength, RowDefinition> HeightProperty =
            ReactiveProperty<GridLength, RowDefinition>.Register("Height", new GridLength(1, GridUnitType.Star));

        public static readonly ReactiveProperty<double, RowDefinition> MaxHeightProperty =
            ReactiveProperty<double, RowDefinition>.Register("MaxHeight", double.PositiveInfinity);

        public static readonly ReactiveProperty<double, RowDefinition> MinHeightProperty =
            ReactiveProperty<double, RowDefinition>.Register("MinHeight", 0d);

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