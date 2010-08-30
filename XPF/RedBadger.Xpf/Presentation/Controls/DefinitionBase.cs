namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Windows;

    public abstract class DefinitionBase : DependencyObject
    {
        private readonly DefinitionType definitionType;

        protected DefinitionBase(DefinitionType definitionType)
        {
            this.definitionType = definitionType;
        }

        protected enum DefinitionType
        {
            Column, 
            Row
        }

        internal double AvailableLength { get; set; }

        internal double FinalLength { get; set; }

        internal double FinalOffset { get; set; }

        internal GridUnitType LengthType { get; set; }

        internal double MinLength { get; set; }

        internal GridLength UserLength
        {
            get
            {
                return
                    (GridLength)
                    this.GetValue(
                        this.definitionType == DefinitionType.Column
                            ? ColumnDefinition.WidthProperty.Value
                            : RowDefinition.HeightProperty.Value);
            }
        }

        internal double UserMaxLength
        {
            get
            {
                return
                    (double)
                    this.GetValue(
                        this.definitionType == DefinitionType.Column
                            ? ColumnDefinition.MaxWidthProperty.Value
                            : RowDefinition.MaxHeightProperty.Value);
            }
        }

        internal double UserMinLength
        {
            get
            {
                return
                    (double)
                    this.GetValue(
                        this.definitionType == DefinitionType.Column
                            ? ColumnDefinition.MinWidthProperty.Value
                            : RowDefinition.MinHeightProperty.Value);
            }
        }

        internal void UpdateMinLength(double minLength)
        {
            this.MinLength = Math.Max(this.MinLength, minLength);
        }
    }
}