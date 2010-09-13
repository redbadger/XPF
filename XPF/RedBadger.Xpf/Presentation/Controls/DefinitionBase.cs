namespace RedBadger.Xpf.Presentation.Controls
{
    using System;

    public abstract class DefinitionBase : ReactiveObject
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
                return this.definitionType == DefinitionType.Column
                           ? this.GetValue(ColumnDefinition.WidthProperty)
                           : this.GetValue(RowDefinition.HeightProperty);
            }
        }

        internal double UserMaxLength
        {
            get
            {
                return this.definitionType == DefinitionType.Column
                           ? this.GetValue(ColumnDefinition.MaxWidthProperty)
                           : this.GetValue(RowDefinition.MaxHeightProperty);
            }
        }

        internal double UserMinLength
        {
            get
            {
                return this.definitionType == DefinitionType.Column
                           ? this.GetValue(ColumnDefinition.MinWidthProperty)
                           : this.GetValue(RowDefinition.MinHeightProperty);
            }
        }

        internal void UpdateMinLength(double minLength)
        {
            this.MinLength = Math.Max(this.MinLength, minLength);
        }
    }
}