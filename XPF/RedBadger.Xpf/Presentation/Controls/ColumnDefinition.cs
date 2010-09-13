namespace RedBadger.Xpf.Presentation.Controls
{
    public class ColumnDefinition : DefinitionBase
    {
        public static readonly ReactiveProperty<double, ColumnDefinition> MaxWidthProperty =
            ReactiveProperty<double, ColumnDefinition>.Register("MaxWidth", double.PositiveInfinity);

        public static readonly ReactiveProperty<double, ColumnDefinition> MinWidthProperty =
            ReactiveProperty<double, ColumnDefinition>.Register("MinWidth", 0d);

        public static readonly ReactiveProperty<GridLength, ColumnDefinition> WidthProperty =
            ReactiveProperty<GridLength, ColumnDefinition>.Register("Width", new GridLength(1, GridUnitType.Star));

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