namespace RedBadger.Xpf.Presentation.Controls
{
    using System;

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
                            ? ColumnDefinition.WidthProperty
                            : RowDefinition.HeightProperty);
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
                            ? ColumnDefinition.MaxWidthProperty
                            : RowDefinition.MaxHeightProperty);
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
                            ? ColumnDefinition.MinWidthProperty
                            : RowDefinition.MinHeightProperty);
            }
        }

        internal void UpdateMinLength(double minLength)
        {
            this.MinLength = Math.Max(this.MinLength, minLength);
        }
    }
}