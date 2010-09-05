namespace RedBadger.Xpf.Presentation.Controls
{
    public class RowDefinition : DefinitionBase
    {
        public static readonly Property<GridLength, RowDefinition> HeightProperty =
            Property<GridLength, RowDefinition>.Register("Height", new GridLength());

        public static readonly Property<double, RowDefinition> MaxHeightProperty =
            Property<double, RowDefinition>.Register("MaxHeight", double.PositiveInfinity);

        public static readonly Property<double, RowDefinition> MinHeightProperty =
            Property<double, RowDefinition>.Register("MinHeight", 0d);

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