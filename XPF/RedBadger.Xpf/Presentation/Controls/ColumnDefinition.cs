namespace RedBadger.Xpf.Presentation.Controls
{
    public class ColumnDefinition : DefinitionBase
    {
        public static readonly IDependencyProperty MaxWidthProperty =
            DependencyProperty<double, ColumnDefinition>.Register(
                "MaxWidth", new PropertyMetadata(double.PositiveInfinity));

        public static readonly IDependencyProperty MinWidthProperty =
            DependencyProperty<double, ColumnDefinition>.Register("MinWidth", new PropertyMetadata(0d));

        public static readonly IDependencyProperty WidthProperty =
            DependencyProperty<GridLength, ColumnDefinition>.Register("Width", new PropertyMetadata(new GridLength()));

        public ColumnDefinition()
            : base(DefinitionType.Column)
        {
        }

        public double MaxWidth
        {
            get
            {
                return this.GetValue<double>(MaxWidthProperty);
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
                return this.GetValue<double>(MinWidthProperty);
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
                return this.GetValue<GridLength>(WidthProperty);
            }

            set
            {
                this.SetValue(WidthProperty, value);
            }
        }
    }
}