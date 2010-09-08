namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RedBadger.Xpf.Internal;

    public class Grid : Panel
    {
        public static readonly ReactiveProperty<int, Grid> ColumnProperty = ReactiveProperty<int, Grid>.Register("Column", 0);

        public static readonly ReactiveProperty<int, Grid> RowProperty = ReactiveProperty<int, Grid>.Register("Row", 0);

        private readonly IList<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();

        private readonly IList<RowDefinition> rowDefinitions = new List<RowDefinition>();

        private Cell[] cells;

        private int cellsWithAllStarsHeadIndex;

        private int cellsWithoutAnyStarsHeadIndex;

        private int cellsWithoutHeightStarsHeadIndex;

        private int cellsWithoutWidthStarsHeadIndex;

        private DefinitionBase[] heightDefinitions;

        private DefinitionBase[] widthDefinitions;

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

        public static int GetColumn(IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return element.GetValue(ColumnProperty);
        }

        public static int GetRow(IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return element.GetValue(RowProperty);
        }

        public static void SetColumn(IElement element, int value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(ColumnProperty, value);
        }

        public static void SetRow(IElement element, int value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(RowProperty, value);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            SetFinalSize(this.widthDefinitions, finalSize.Width);
            SetFinalSize(this.heightDefinitions, finalSize.Height);

            for (int i = 0; i < this.cells.Length; i++)
            {
                IElement child = this.Children[i];
                if (child != null)
                {
                    int columnIndex = this.cells[i].ColumnIndex;
                    int rowIndex = this.cells[i].RowIndex;

                    var finalRect = new Rect(
                        this.widthDefinitions[columnIndex].FinalOffset, 
                        this.heightDefinitions[rowIndex].FinalOffset, 
                        this.widthDefinitions[columnIndex].FinalLength, 
                        this.heightDefinitions[rowIndex].FinalLength);

                    child.Arrange(finalRect);
                }
            }

            return finalSize;
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
                definition.MinLength = 0;
                double availableLength = 0;
                double userMinLength = definition.UserMinLength;
                double userMaxLength = definition.UserMaxLength;

                switch (definition.UserLength.GridUnitType)
                {
                    case GridUnitType.Auto:
                        definition.LengthType = GridUnitType.Auto;
                        availableLength = double.PositiveInfinity;
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

        private static void SetFinalSize(DefinitionBase[] definitions, double finalLength)
        {
            double cumulativeLength = 0.0;

            foreach (DefinitionBase definition in definitions)
            {
                double minLength = 0.0;
                switch (definition.UserLength.GridUnitType)
                {
                    case GridUnitType.Auto:
                        minLength = definition.MinLength;
                        break;

                    case GridUnitType.Pixel:
                        minLength = definition.UserLength.Value;
                        break;
                }

                definition.FinalLength = Math.Max(definition.MinLength, Math.Min(minLength, definition.UserMaxLength));
                cumulativeLength += definition.FinalLength;
            }

            if (cumulativeLength.IsGreaterThan(finalLength))
            {
                // TODO: deal with redistributing the extra length when gridlenth star is implemented
            }

            definitions[0].FinalOffset = 0.0;
            for (int i = 1; i < definitions.Length; i++)
            {
                DefinitionBase previousDefinition = definitions[i - 1];
                definitions[i].FinalOffset = previousDefinition.FinalOffset + previousDefinition.FinalLength;
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
                IElement element = this.Children[i];
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
            IElement child = this.Children[cellIndex];
            if (child != null)
            {
                Cell cell = this.cells[cellIndex];

                double x = cell.WidthType == GridUnitType.Auto
                               ? double.PositiveInfinity
                               : this.widthDefinitions[cell.ColumnIndex].AvailableLength;

                double y = cell.HeightType == GridUnitType.Auto
                               ? double.PositiveInfinity
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

                    Cell cell = this.cells[currentCellIndex];
                    IElement child = this.Children[currentCellIndex];

                    DefinitionBase widthDefinition = this.widthDefinitions[cell.ColumnIndex];
                    widthDefinition.UpdateMinLength(Math.Min(child.DesiredSize.Width, widthDefinition.UserMaxLength));

                    DefinitionBase heightDefinition = this.heightDefinitions[cell.RowIndex];
                    heightDefinition.UpdateMinLength(Math.Min(child.DesiredSize.Height, heightDefinition.UserMaxLength));

                    currentCellIndex = cell.Next;
                }
                while (currentCellIndex < this.cells.Length);
            }
        }

        private struct Cell
        {
            public int ColumnIndex;

            public GridUnitType HeightType;

            public int Next;

            public int RowIndex;

            public GridUnitType WidthType;
        }
    }
}