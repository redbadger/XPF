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

        public static readonly DependencyProperty MaxHeightProperty = DependencyProperty.Register(
            "MaxHeight", typeof(float), typeof(RowDefinition), new PropertyMetadata(float.PositiveInfinity));

        public static readonly DependencyProperty MinHeightProperty = DependencyProperty.Register(
            "MinHeight", typeof(float), typeof(RowDefinition), new PropertyMetadata(0f));

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

        public float MaxHeight
        {
            get
            {
                return (float)this.GetValue(MaxHeightProperty);
            }

            set
            {
                this.SetValue(MaxHeightProperty, value);
            }
        }

        public float MinHeight
        {
            get
            {
                return (float)this.GetValue(MinHeightProperty);
            }

            set
            {
                this.SetValue(MinHeightProperty, value);
            }
        }
    }
}