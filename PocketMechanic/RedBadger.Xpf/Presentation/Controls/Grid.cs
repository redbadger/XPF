namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using Size = RedBadger.Xpf.Presentation.Size;

#if WINDOWS_PHONE
    using UIElement = RedBadger.Xpf.Presentation.UIElement;
    using GridUnitType = RedBadger.Xpf.Presentation.GridUnitType;
#endif

    public class Grid : Panel
    {
        public static readonly DependencyProperty ColumnProperty = DependencyProperty.RegisterAttached(
            "Column", typeof(int), typeof(Grid), new PropertyMetadata(0));

        public static readonly DependencyProperty RowProperty = DependencyProperty.RegisterAttached(
            "Row", typeof(int), typeof(Grid), new PropertyMetadata(0));

        private readonly IList<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();

        private readonly IList<RowDefinition> rowDefinitions = new List<RowDefinition>();

        private Cell[] cells;

        private int cellsWithoutAnyStarsHeadIndex;

        private DefinitionBase[] heightDefinitions;

        private DefinitionBase[] widthDefinitions;

        private int cellsWithoutHeightStarsHeadIndex;

        private int cellsWithoutWidthStarsHeadIndex;

        private int cellsWithAllStarsHeadIndex;

        public IList<ColumnDefinition> ColumnDefinitions
        {
            get
            {
                return this.columnDefinitions;
            }
        }

        public IList<RowDefinition> RowDefinitions
        {
            get
            {
                return this.rowDefinitions;
            }
        }

        public static int GetColumn(UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (int)element.GetValue(ColumnProperty);
        }

        public static int GetRow(UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (int)element.GetValue(RowProperty);
        }

        public static void SetColumn(UIElement element, int value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(ColumnProperty, value);
        }

        public static void SetRow(UIElement element, int value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(RowProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.widthDefinitions = this.columnDefinitions.Count == 0
                                        ? new DefinitionBase[] { new ColumnDefinition() }
                                        : this.columnDefinitions.ToArray();

            this.heightDefinitions = this.rowDefinitions.Count == 0
                                         ? new DefinitionBase[] { new RowDefinition() }
                                         : this.rowDefinitions.ToArray();

            InitializeMeasureData(this.widthDefinitions);
            InitializeMeasureData(this.heightDefinitions);
            this.CreateCells();
            this.MeasureCellGroup(this.cellsWithoutAnyStarsHeadIndex);

            return new Size(
                this.widthDefinitions.Sum(definition => definition.MinLength),
                this.heightDefinitions.Sum(definition => definition.MinLength));
        }

        private static void InitializeMeasureData(IEnumerable<DefinitionBase> definitions)
        {
            foreach (DefinitionBase definition in definitions)
            {
                var availableLength = 0f;
                float userMinLength = definition.UserMinLength;
                float userMaxLength = definition.UserMaxLength;

                switch (definition.UserLength.GridUnitType)
                {
                    case GridUnitType.Auto:
                        definition.LengthType = GridUnitType.Auto;
                        availableLength = float.PositiveInfinity;
                        break;
                    case GridUnitType.Pixel:
                        definition.LengthType = GridUnitType.Pixel;
                        availableLength = definition.UserLength.Value;
                        userMinLength = Math.Max(userMinLength, Math.Min(availableLength, userMaxLength));
                        break;
                }

                definition.UpdateMinLength(userMinLength);
                definition.AvailableLength = Math.Max(userMinLength, Math.Min(availableLength, userMaxLength));
            }
        }

        private void CreateCells()
        {
            this.cells = new Cell[this.Children.Count];
            this.cellsWithoutAnyStarsHeadIndex = int.MaxValue;
            this.cellsWithoutHeightStarsHeadIndex = int.MaxValue;
            this.cellsWithoutWidthStarsHeadIndex = int.MaxValue;
            this.cellsWithAllStarsHeadIndex = int.MaxValue;

            for (int i = this.cells.Length - 1; i >= 0; i--)
            {
                UIElement element = this.Children[i];
                if (element != null)
                {
                    var cell = new Cell
                        {
                            ColumnIndex = GetColumn(element), 
                            RowIndex = GetRow(element), 
                            Next = this.cellsWithoutAnyStarsHeadIndex
                        };
                    cell.WidthType = this.widthDefinitions[cell.ColumnIndex].LengthType;
                    cell.HeightType = this.heightDefinitions[cell.RowIndex].LengthType;

                    if (cell.HeightType != GridUnitType.Star)
                    {
                        if (cell.WidthType != GridUnitType.Star)
                        {
                            cell.Next = this.cellsWithoutAnyStarsHeadIndex;
                            this.cellsWithoutAnyStarsHeadIndex = i;
                        }
                        else
                        {
                            cell.Next = this.cellsWithoutHeightStarsHeadIndex;
                            this.cellsWithoutHeightStarsHeadIndex = i;
                            
                            // hasGroup3CellsInAutoRows |= cell.SizeTypeV == GridUnitType.Auto;
                        }
                    }
                    else if (cell.WidthType != GridUnitType.Star)
                    {
                        cell.Next = this.cellsWithoutWidthStarsHeadIndex;
                        this.cellsWithoutWidthStarsHeadIndex = i;
                    }
                    else
                    {
                        cell.Next = this.cellsWithAllStarsHeadIndex;
                        this.cellsWithAllStarsHeadIndex = i;
                    }

                    this.cells[i] = cell;
                }
            }
        }

        private void MeasureCell(int cellIndex)
        {
            UIElement child = this.Children[cellIndex];
            if (child != null)
            {
                Cell cell = this.cells[cellIndex];

                float x = cell.WidthType == GridUnitType.Auto
                              ? float.PositiveInfinity
                              : this.widthDefinitions[cell.ColumnIndex].AvailableLength;

                float y = cell.HeightType == GridUnitType.Auto
                              ? float.PositiveInfinity
                              : this.heightDefinitions[cell.RowIndex].AvailableLength;

                child.Measure(new Size(x, y));
            }
        }

        private void MeasureCellGroup(int headCellIndex)
        {
            if (headCellIndex < this.cells.Length)
            {
                int currentCellIndex = headCellIndex;

                do
                {
                    this.MeasureCell(currentCellIndex);

                    var cell = this.cells[currentCellIndex];
                    var child = this.Children[currentCellIndex];

                    var widthDefinition = this.widthDefinitions[cell.ColumnIndex];
                    widthDefinition.UpdateMinLength(Math.Min(child.DesiredSize.Width, widthDefinition.UserMaxLength));

                    var heightDefinition = this.heightDefinitions[cell.RowIndex];
                    heightDefinition.UpdateMinLength(Math.Min(child.DesiredSize.Height, heightDefinition.UserMaxLength));

                    currentCellIndex = cell.Next;
                }
                while (currentCellIndex < this.cells.Length);
            }
        }

        private struct Cell
        {
            public int ColumnIndex;

            public int Next;

            public int RowIndex;

            public GridUnitType WidthType;

            public GridUnitType HeightType;
        }
    }
}