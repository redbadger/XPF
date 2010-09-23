namespace RedBadger.Xpf.Controls
{
    public class ColumnDefinition : DefinitionBase
    {
        public static readonly ReactiveProperty<double> MaxWidthProperty = ReactiveProperty<double>.Register(
            "MaxWidth", typeof(ColumnDefinition), double.PositiveInfinity);

        public static readonly ReactiveProperty<double> MinWidthProperty = ReactiveProperty<double>.Register(
            "MinWidth", typeof(ColumnDefinition), 0d);

        public static readonly ReactiveProperty<GridLength> WidthProperty =
            ReactiveProperty<GridLength>.Register(
                "Width", typeof(ColumnDefinition), new GridLength(1, GridUnitType.Star));

        public ColumnDefinition()
            : base(DefinitionType.Column)
        {
        }

        public double MaxWidth
        {
            get
            {
                return this.GetValue(MaxWidthProperty);
            }

            set
            {
                this.SetValue(MaxWidthProperty, value);
            }
        }

        public double MinWidth
        {
            get
            {
                return this.GetValue(MinWidthProperty);
            }

            set
            {
                this.SetValue(MinWidthProperty, value);
            }
        }

        public GridLength Width
        {
            get
            {
                return this.GetValue(WidthProperty);
            }

            set
            {
                this.SetValue(WidthProperty, value);
            }
        }
    }
}