namespace RedBadger.Xpf.Presentation.Controls
{
    public class RowDefinition : DefinitionBase
    {
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
            "Height", typeof(GridLength), typeof(RowDefinition), new PropertyMetadata(new GridLength()));

        public static readonly DependencyProperty MaxHeightProperty = DependencyProperty.Register(
            "MaxHeight", typeof(double), typeof(RowDefinition), new PropertyMetadata(double.PositiveInfinity));

        public static readonly DependencyProperty MinHeightProperty = DependencyProperty.Register(
            "MinHeight", typeof(double), typeof(RowDefinition), new PropertyMetadata(0d));

        public RowDefinition()
            : base(DefinitionType.Row)
        {
        }

        public GridLength Height
        {
            get
            {
                return (GridLength)this.GetValue(HeightProperty);
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
                return (double)this.GetValue(MaxHeightProperty);
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
                return (double)this.GetValue(MinHeightProperty);
            }

            set
            {
                this.SetValue(MinHeightProperty, value);
            }
        }
    }
}