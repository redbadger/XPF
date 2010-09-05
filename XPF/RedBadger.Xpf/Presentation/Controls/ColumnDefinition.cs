namespace RedBadger.Xpf.Presentation.Controls
{
    public class ColumnDefinition : DefinitionBase
    {
        public static readonly Property<double, ColumnDefinition> MaxWidthProperty =
            Property<double, ColumnDefinition>.Register("MaxWidth", double.PositiveInfinity);

        public static readonly Property<double, ColumnDefinition> MinWidthProperty =
            Property<double, ColumnDefinition>.Register("MinWidth", 0d);

        public static readonly Property<GridLength, ColumnDefinition> WidthProperty =
            Property<GridLength, ColumnDefinition>.Register("Width", new GridLength());

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