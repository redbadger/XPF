namespace RedBadger.Xpf.Presentation.Controls
{
    public class RowDefinition : DefinitionBase
    {
        public static readonly IDependencyProperty HeightProperty =
            DependencyProperty<GridLength, RowDefinition>.Register("Height", new PropertyMetadata(new GridLength()));

        public static readonly IDependencyProperty MaxHeightProperty =
            DependencyProperty<double, RowDefinition>.Register(
                "MaxHeight", new PropertyMetadata(double.PositiveInfinity));

        public static readonly IDependencyProperty MinHeightProperty =
            DependencyProperty<double, RowDefinition>.Register("MinHeight", new PropertyMetadata(0d));

        public RowDefinition()
            : base(DefinitionType.Row)
        {
        }

        public GridLength Height
        {
            get
            {
                return this.GetValue<GridLength>(HeightProperty);
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
                return this.GetValue<double>(MaxHeightProperty);
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
                return this.GetValue<double>(MinHeightProperty);
            }

            set
            {
                this.SetValue(MinHeightProperty, value);
            }
        }
    }
}