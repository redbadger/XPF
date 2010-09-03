namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using RedBadger.Xpf.Internal;

    public class Grid : Panel
    {
        public static readonly XpfDependencyProperty ColumnProperty = XpfDependencyProperty.RegisterAttached(
            "Column", typeof(int), typeof(Grid), new PropertyMetadata(0));

        public static readonly XpfDependencyProperty RowProperty = XpfDependencyProperty.RegisterAttached(
            "Row", typeof(int), typeof(Grid), new PropertyMetadata(0));

        private static readonly StarDistributionComparerByFinalLengths compareDefinitionByFinalLengths =
            new StarDistributionComparerByFinalLengths();

        private readonly IList<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();

        private readonly IList<RowDefinition> rowDefinitions = new List<RowDefinition>();

        private bool areThereAnyCellsWithStarHeights;

        private bool areThereAnyCellsWithStarWidths;

        private Cell[] cells;

        private int cellsWithAllStarsHeadIndex;

        private int cellsWithoutAnyStarsHeadIndex;

        private int cellsWithoutHeightStarsHeadIndex;

        private int cellsWithoutWidthStarsHeadIndex;

        private bool doAnyCellsWithoutStarHeightHaveAutoWidth;

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

        public static int GetColumn(IDependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            return (int)dependencyObject.GetValue(ColumnProperty.Value);
        }

        public static int GetRow(IDependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            return (int)dependencyObject.GetValue(RowProperty.Value);
        }

        public static void SetColumn(IDependencyObject dependencyObject, int value)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            dependencyObject.SetValue(ColumnProperty.Value, value);
        }

        public static void SetRow(IDependencyObject dependencyObject, int value)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            dependencyObject.SetValue(RowProperty.Value, value);
        }

        internal static bool CompareNullRefs(object x, object y, out int result)
        {
            result = 2;
            if (x == null)
            {
                if (y == null)
                {
                    result = 0;
                }
                else
                {
                    result = -1;
                }
            }
            else if (y == null)
            {
                result = 1;
            }

            return result != 2;
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

            bool treatStarAsAutoWidth = double.IsPositiveInfinity(availableSize.Width);
            bool treatStarAsAutoHeight = double.IsPositiveInfinity(availableSize.Height);

            InitializeMeasureData(this.widthDefinitions, treatStarAsAutoWidth);
            InitializeMeasureData(this.heightDefinitions, treatStarAsAutoHeight);

            this.CreateCells();

            this.MeasureCellGroup(this.cellsWithoutAnyStarsHeadIndex, false, false);

            if (!this.doAnyCellsWithoutStarHeightHaveAutoWidth)
            {
                if (this.areThereAnyCellsWithStarHeights)
                {
                    ResolveStar(this.heightDefinitions, availableSize.Height);
                }

                this.MeasureCellGroup(this.cellsWithoutWidthStarsHeadIndex, false, false);

                if (this.areThereAnyCellsWithStarWidths)
                {
                    ResolveStar(this.widthDefinitions, availableSize.Width);
                }

                this.MeasureCellGroup(this.cellsWithoutHeightStarsHeadIndex, false, false);
            }
            else if (this.cellsWithoutWidthStarsHeadIndex > this.cells.Length)
            {
                if (this.areThereAnyCellsWithStarWidths)
                {
                    ResolveStar(this.widthDefinitions, availableSize.Width);
                }

                this.MeasureCellGroup(this.cellsWithoutHeightStarsHeadIndex, false, false);

                if (this.areThereAnyCellsWithStarHeights)
                {
                    ResolveStar(this.heightDefinitions, availableSize.Height);
                }
            }
            else
            {
                this.MeasureCellGroup(this.cellsWithoutWidthStarsHeadIndex, false, true);

                if (this.areThereAnyCellsWithStarWidths)
                {
                    ResolveStar(this.widthDefinitions, availableSize.Width);
                }

                this.MeasureCellGroup(this.cellsWithoutHeightStarsHeadIndex, false, false);

                if (this.areThereAnyCellsWithStarHeights)
                {
                    ResolveStar(this.heightDefinitions, availableSize.Height);
                }

                this.MeasureCellGroup(this.cellsWithoutWidthStarsHeadIndex, true, false);
            }

            this.MeasureCellGroup(this.cellsWithAllStarsHeadIndex, false, false);

            return new Size(
                this.widthDefinitions.Sum(definition => definition.MinLength), 
                this.heightDefinitions.Sum(definition => definition.MinLength));
        }

        private static void InitializeMeasureData(IEnumerable<DefinitionBase> definitions, bool treatStarAsAuto)
        {
            foreach (DefinitionBase definition in definitions)
            {
                definition.MinLength = 0d;
                double availableLength;
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
                    case GridUnitType.Star:
                        definition.LengthType = treatStarAsAuto ? GridUnitType.Auto : GridUnitType.Star;
                        availableLength = double.PositiveInfinity;
                        break;
                    default:
                        throw new NotSupportedException("Unsupported GridUnitType");
                }

                definition.UpdateMinLength(userMinLength);
                definition.AvailableLength = Math.Max(userMinLength, Math.Min(availableLength, userMaxLength));
            }
        }

        private static void ResolveStar(IEnumerable<DefinitionBase> definitions, double availableSize)
        {
            double occupiedSpace = 0.0;

            var stars = new List<DefinitionBase>();

            foreach (DefinitionBase definition in definitions)
            {
                switch (definition.LengthType)
                {
                    case GridUnitType.Pixel:
                        occupiedSpace += definition.AvailableLength;
                        break;

                    case GridUnitType.Auto:
                        occupiedSpace += definition.MinLength;
                        break;

                    case GridUnitType.Star:
                        stars.Add(definition);
                        double divisor = definition.UserLength.Value;
                        if (divisor.IsCloseTo(0))
                        {
                            definition.AvailableLength = 0.0;
                            definition.FinalLength = 0.0;
                        }
                        else
                        {
                            definition.AvailableLength = divisor;
                            definition.FinalLength = Math.Max(definition.MinLength, definition.UserMaxLength) / divisor;
                        }

                        break;
                }
            }

            if (stars.Count > 0)
            {
                stars.Sort(compareDefinitionByFinalLengths);

                double cumulativeMeasureSize = 0.0;
                foreach (DefinitionBase definition in Enumerable.Reverse(stars))
                {
                    cumulativeMeasureSize += definition.AvailableLength;
                    definition.FinalLength = cumulativeMeasureSize;
                }

                foreach (DefinitionBase definition in stars)
                {
                    double size;
                    double divisor = definition.AvailableLength;
                    if (divisor.IsCloseTo(0))
                    {
                        size = definition.MinLength;
                    }
                    else
                    {
                        size = Math.Max(availableSize - occupiedSpace, 0.0) * (divisor / definition.FinalLength);
                        size = Math.Max(definition.MinLength, Math.Min(size, definition.UserMaxLength));
                    }

                    definition.AvailableLength = size;
                    occupiedSpace += size;
                }
            }
        }

        private static void SetFinalSize(DefinitionBase[] definitions, double finalLength)
        {
            double cumulativeLength = 0.0;

            foreach (DefinitionBase definition in definitions)
            {
                double minLength;
                switch (definition.UserLength.GridUnitType)
                {
                    case GridUnitType.Auto:
                        minLength = definition.MinLength;
                        break;
                    case GridUnitType.Pixel:
                        minLength = definition.UserLength.Value;
                        break;
                    default:
                        throw new NotSupportedException("Unsupported GridUnitType");
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

            this.areThereAnyCellsWithStarWidths = false;
            this.areThereAnyCellsWithStarHeights = false;
            this.doAnyCellsWithoutStarHeightHaveAutoWidth = false;

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
                    this.areThereAnyCellsWithStarWidths |= cell.WidthType == GridUnitType.Star;
                    this.areThereAnyCellsWithStarHeights |= cell.HeightType == GridUnitType.Star;

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

                            this.doAnyCellsWithoutStarHeightHaveAutoWidth |= cell.HeightType == GridUnitType.Auto;
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

        private void MeasureCell(int cellIndex, bool shouldForceInfinityHeight)
        {
            IElement child = this.Children[cellIndex];
            if (child != null)
            {
                Cell cell = this.cells[cellIndex];

                double x = cell.WidthType == GridUnitType.Auto
                               ? double.PositiveInfinity
                               : this.widthDefinitions[cell.ColumnIndex].AvailableLength;

                double y = cell.HeightType == GridUnitType.Auto || shouldForceInfinityHeight
                               ? double.PositiveInfinity
                               : this.heightDefinitions[cell.RowIndex].AvailableLength;

                child.Measure(new Size(x, y));
            }
        }

        private void MeasureCellGroup(int headCellIndex, bool skipUpdateMinWidth, bool skipUpdateMinHeight)
        {
            if (headCellIndex < this.cells.Length)
            {
                int currentCellIndex = headCellIndex;

                do
                {
                    bool shouldForceInfinityHeight = skipUpdateMinHeight;
                    this.MeasureCell(currentCellIndex, shouldForceInfinityHeight);

                    Cell cell = this.cells[currentCellIndex];
                    IElement child = this.Children[currentCellIndex];

                    if (!skipUpdateMinWidth)
                    {
                        DefinitionBase widthDefinition = this.widthDefinitions[cell.ColumnIndex];
                        widthDefinition.UpdateMinLength(
                            Math.Min(child.DesiredSize.Width, widthDefinition.UserMaxLength));
                    }

                    if (!skipUpdateMinHeight)
                    {
                        DefinitionBase heightDefinition = this.heightDefinitions[cell.RowIndex];
                        heightDefinition.UpdateMinLength(
                            Math.Min(child.DesiredSize.Height, heightDefinition.UserMaxLength));
                    }

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

        private class StarDistributionComparerByFinalLengths : IComparer<DefinitionBase>
        {
            public int Compare(DefinitionBase x, DefinitionBase y)
            {
                int num;
                if (!CompareNullRefs(x, y, out num))
                {
                    num = x.FinalLength.CompareTo(y.FinalLength);
                }

                return num;
            }
        }
    }
}