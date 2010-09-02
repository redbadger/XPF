namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    public class RowDefinition : DefinitionBase
    {
        public static readonly XpfDependencyProperty HeightProperty = XpfDependencyProperty.Register(
            "Height", typeof(GridLength), typeof(RowDefinition), new PropertyMetadata(new GridLength(1, GridUnitType.Star)));

        public static readonly XpfDependencyProperty MaxHeightProperty = XpfDependencyProperty.Register(
            "MaxHeight", typeof(double), typeof(RowDefinition), new PropertyMetadata(double.PositiveInfinity));

        public static readonly XpfDependencyProperty MinHeightProperty = XpfDependencyProperty.Register(
            "MinHeight", typeof(double), typeof(RowDefinition), new PropertyMetadata(0d));

        public RowDefinition()
            : base(DefinitionType.Row)
        {
        }

        public GridLength Height
        {
            get
            {
                return (GridLength)this.GetValue(HeightProperty.Value);
            }

            set
            {
                this.SetValue(HeightProperty.Value, value);
            }
        }

        public double MaxHeight
        {
            get
            {
                return (double)this.GetValue(MaxHeightProperty.Value);
            }

            set
            {
                this.SetValue(MaxHeightProperty.Value, value);
            }
        }

        public double MinHeight
        {
            get
            {
                return (double)this.GetValue(MinHeightProperty.Value);
            }

            set
            {
                this.SetValue(MinHeightProperty.Value, value);
            }
        }
    }
}