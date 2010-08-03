namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

#if WINDOWS_PHONE
    using GridLength = RedBadger.Xpf.Presentation.GridLength;
#endif

    public class RowDefinition : DefinitionBase
    {
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
            "Height", typeof(GridLength), typeof(ColumnDefinition), new PropertyMetadata(new GridLength()));

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
    }
}