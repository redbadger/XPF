namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using GridLength = RedBadger.Xpf.Presentation.GridLength;

    public class ColumnDefinition : DefinitionBase
    {
        public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register(
            "MaxWidth", typeof(float), typeof(ColumnDefinition), new PropertyMetadata(float.PositiveInfinity));

        public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register(
            "MinWidth", typeof(float), typeof(ColumnDefinition), new PropertyMetadata(0f));

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            "Width", typeof(GridLength), typeof(ColumnDefinition), new PropertyMetadata(new GridLength()));

        public ColumnDefinition()
            : base(DefinitionType.Column)
        {
        }

        public float MaxWidth
        {
            get
            {
                return (float)this.GetValue(MaxWidthProperty);
            }

            set
            {
                this.SetValue(MaxWidthProperty, value);
            }
        }

        public float MinWidth
        {
            get
            {
                return (float)this.GetValue(MinWidthProperty);
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