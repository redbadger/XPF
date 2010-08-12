namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using GridLength = RedBadger.Xpf.Presentation.GridLength;

    public class ColumnDefinition : DefinitionBase
    {
        public static readonly XpfDependencyProperty MaxWidthProperty = XpfDependencyProperty.Register(
            "MaxWidth", typeof(float), typeof(ColumnDefinition), new PropertyMetadata(float.PositiveInfinity));

        public static readonly XpfDependencyProperty MinWidthProperty = XpfDependencyProperty.Register(
            "MinWidth", typeof(float), typeof(ColumnDefinition), new PropertyMetadata(0f));

        public static readonly XpfDependencyProperty WidthProperty = XpfDependencyProperty.Register(
            "Width", typeof(GridLength), typeof(ColumnDefinition), new PropertyMetadata(new GridLength()));

        public ColumnDefinition()
            : base(DefinitionType.Column)
        {
        }

        public float MaxWidth
        {
            get
            {
                return (float)this.GetValue(MaxWidthProperty.Value);
            }

            set
            {
                this.SetValue(MaxWidthProperty.Value, value);
            }
        }

        public float MinWidth
        {
            get
            {
                return (float)this.GetValue(MinWidthProperty.Value);
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