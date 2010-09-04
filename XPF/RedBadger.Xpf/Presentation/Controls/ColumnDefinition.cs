namespace RedBadger.Xpf.Presentation.Controls
{
    public class ColumnDefinition : DefinitionBase
    {
        public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register(
            "MaxWidth", typeof(double), typeof(ColumnDefinition), new PropertyMetadata(double.PositiveInfinity));

        public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register(
            "MinWidth", typeof(double), typeof(ColumnDefinition), new PropertyMetadata(0d));

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            "Width", typeof(GridLength), typeof(ColumnDefinition), new PropertyMetadata(new GridLength()));

        public ColumnDefinition()
            : base(DefinitionType.Column)
        {
        }

        public double MaxWidth
        {
            get
            {
                return (double)this.GetValue(MaxWidthProperty);
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
                return (double)this.GetValue(MinWidthProperty);
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
                return (GridLength)this.GetValue(WidthProperty);
            }

            set
            {
                this.SetValue(WidthProperty, value);
            }
        }
    }
}