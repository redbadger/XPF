namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    public class ColumnDefinition : DefinitionBase
    {
        public static readonly XpfDependencyProperty MaxWidthProperty = XpfDependencyProperty.Register(
            "MaxWidth", typeof(double), typeof(ColumnDefinition), new PropertyMetadata(double.PositiveInfinity));

        public static readonly XpfDependencyProperty MinWidthProperty = XpfDependencyProperty.Register(
            "MinWidth", typeof(double), typeof(ColumnDefinition), new PropertyMetadata(0d));

        public static readonly XpfDependencyProperty WidthProperty = XpfDependencyProperty.Register(
            "Width", typeof(GridLength), typeof(ColumnDefinition), new PropertyMetadata(new GridLength(1, GridUnitType.Star)));

        public ColumnDefinition()
            : base(DefinitionType.Column)
        {
        }

        public double MaxWidth
        {
            get
            {
                return (double)this.GetValue(MaxWidthProperty.Value);
            }

            set
            {
                this.SetValue(MaxWidthProperty.Value, value);
            }
        }

        public double MinWidth
        {
            get
            {
                return (double)this.GetValue(MinWidthProperty.Value);
            }

            set
            {
                this.SetValue(MinWidthProperty.Value, value);
            }
        }

        public GridLength Width
        {
            get
            {
                return (GridLength)this.GetValue(WidthProperty.Value);
            }

            set
            {
                this.SetValue(WidthProperty.Value, value);
            }
        }
    }
}