namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

#if WINDOWS_PHONE
    using GridLength = RedBadger.Xpf.Presentation.GridLength;
#endif

    public class ColumnDefinition : DefinitionBase
    {
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            "Width", typeof(GridLength), typeof(ColumnDefinition), new PropertyMetadata(new GridLength()));

        public ColumnDefinition()
            : base(DefinitionType.Column)
        {
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