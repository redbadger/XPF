namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Windows;

    using GridLength = RedBadger.Xpf.Presentation.GridLength;
    using GridUnitType = RedBadger.Xpf.Presentation.GridUnitType;

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

        public GridUnitType LengthType { get; set; }

        internal float AvailableLength { get; set; }

        internal float FinalLength { get; set; }

        internal float FinalOffset { get; set; }

        internal float MinLength { get; private set; }

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

        internal float UserMaxLength
        {
            get
            {
                return
                    (float)
                    this.GetValue(
                        this.definitionType == DefinitionType.Column
                            ? ColumnDefinition.MaxWidthProperty
                            : RowDefinition.MaxHeightProperty);
            }
        }

        internal float UserMinLength
        {
            get
            {
                return
                    (float)
                    this.GetValue(
                        this.definitionType == DefinitionType.Column
                            ? ColumnDefinition.MinWidthProperty
                            : RowDefinition.MinHeightProperty);
            }
        }

        internal void UpdateMinLength(float minLength)
        {
            this.MinLength = Math.Max(this.MinLength, minLength);
        }
    }
}