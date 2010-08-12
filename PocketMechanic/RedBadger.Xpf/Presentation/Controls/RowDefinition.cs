namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using GridLength = RedBadger.Xpf.Presentation.GridLength;

    public class RowDefinition : DefinitionBase
    {
        public static readonly XpfDependencyProperty HeightProperty = XpfDependencyProperty.Register(
            "Height", typeof(GridLength), typeof(RowDefinition), new PropertyMetadata(new GridLength()));

        public static readonly XpfDependencyProperty MaxHeightProperty = XpfDependencyProperty.Register(
            "MaxHeight", typeof(float), typeof(RowDefinition), new PropertyMetadata(float.PositiveInfinity));

        public static readonly XpfDependencyProperty MinHeightProperty = XpfDependencyProperty.Register(
            "MinHeight", typeof(float), typeof(RowDefinition), new PropertyMetadata(0f));

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

        public float MaxHeight
        {
            get
            {
                return (float)this.GetValue(MaxHeightProperty.Value);
            }

            set
            {
                this.SetValue(MaxHeightProperty.Value, value);
            }
        }

        public float MinHeight
        {
            get
            {
                return (float)this.GetValue(MinHeightProperty.Value);
            }

            set
            {
                this.SetValue(MinHeightProperty.Value, value);
            }
        }
    }
}