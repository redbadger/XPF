namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RedBadger.Xpf.Internal;

    /// <summary>
    /// A Grid layout panel consisting of columns and rows.
    /// </summary>
    public class Grid : Panel
    {
        /// <summary>
        /// Column attached property.
        /// </summary>
        public static readonly ReactiveProperty<int, Grid> ColumnProperty =
            ReactiveProperty<int, Grid>.Register("Column");

        /// <summary>
        /// Row attached property.
        /// </summary>
        public static readonly ReactiveProperty<int, Grid> RowProperty = ReactiveProperty<int, Grid>.Register("Row");

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

        /// <summary>
        /// Gets the collection of column definitions.
        /// </summary>
        /// <value>The column definitions collection.</value>
        public IList<ColumnDefinition> ColumnDefinitions
        {
            get
            {
                return this.columnDefinitions;
            }
        }

        /// <summary>
        /// Gets the collection of row definitions.
        /// </summary>
        /// <value>The row definitions collection.</value>
        public IList<RowDefinition> RowDefinitions
        {
            get
            {
                return this.rowDefinitions;
            }
        }

        /// <summary>
        /// Gets the value of the Column attached property for the specified element.
        /// </summary>
        /// <param name="element">The element for which to read the proerty value.</param>
        /// <returns>The value of the Column attached property.</returns>
        public static int GetColumn(IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return element.GetValue(ColumnProperty);
        }

        /// <summary>
        /// Gets the value of the Row attached property for the specified element.
        /// </summary>
        /// <param name="element">The element for which to read the proerty value.</param>
        /// <returns>The value of the Row attached property.</returns>
        public static int GetRow(IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return element.GetValue(RowProperty);
        }

        /// <summary>
        /// Sets the value of the Column attached property for the specified element.
        /// </summary>
        /// <param name="element">The element for which to write the proerty value.</param>
        /// <param name="value">The value of the Column attached property.</param>
        public static void SetColumn(IElement element, int value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(ColumnProperty, value);
        }

        /// <summary>
        /// Sets the value of the Row attached property for the specified element.
        /// </summary>
        /// <param name="element">The element for which to write the proerty value.</param>
        /// <param name="value">The value of the Row attached property.</param>
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
            SetFinalLength(this.widthDefinitions, finalSize.Width);
            SetFinalLength(this.heightDefinitions, finalSize.Height);

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

        private static void ResolveStar(IEnumerable<DefinitionBase> definitions, double availableLength)
        {
            double occupiedSpace = 0.0;

            var stars = new LinkedList<DefinitionBase>();

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
                        stars.AddLast(definition);
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
                var sortedStars = stars.OrderBy(o => o.FinalLength).ToArray();

                double cumulativeDivisor = 0.0;
                foreach (DefinitionBase definition in sortedStars.Reverse())
                {
                    cumulativeDivisor += definition.AvailableLength;
                    definition.FinalLength = cumulativeDivisor;
                }

                foreach (DefinitionBase definition in sortedStars)
                {
                    double size;
                    double divisor = definition.AvailableLength;
                    if (divisor.IsCloseTo(0))
                    {
                        size = definition.MinLength;
                    }
                    else
                    {
                        size = (availableLength - occupiedSpace).EnsurePositive() * (divisor / definition.FinalLength);
                        size = Math.Max(definition.MinLength, Math.Min(size, definition.UserMaxLength));
                    }

                    definition.AvailableLength = size;
                    occupiedSpace += size;
                }
            }
        }

        private static void SetFinalLength(DefinitionBase[] definitions, double gridFinalLength)
        {
            double cumulativeLength = 0.0;

            var starDefinitions = new LinkedList<DefinitionBase>();
            var nonStarDefinitions = new LinkedList<DefinitionBase>();

            foreach (DefinitionBase definition in definitions)
            {
                double minLength;

                switch (definition.UserLength.GridUnitType)
                {
                    case GridUnitType.Auto:
                        minLength = definition.MinLength;

                        definition.FinalLength = minLength.Coerce(definition.MinLength, definition.UserMaxLength);

                        cumulativeLength += definition.FinalLength;
                        nonStarDefinitions.AddFirst(definition);

                        break;
                    case GridUnitType.Pixel:
                        minLength = definition.UserLength.Value;

                        definition.FinalLength = minLength.Coerce(definition.MinLength, definition.UserMaxLength);

                        cumulativeLength += definition.FinalLength;
                        nonStarDefinitions.AddFirst(definition);

                        break;
                    case GridUnitType.Star:
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

                        starDefinitions.AddLast(definition);

                        break;
                    default:
                        throw new NotSupportedException("Unsupported GridUnitType");
                }
            }

            if (starDefinitions.Count > 0)
            {
                var sortedStars = starDefinitions.OrderBy(o => o.FinalLength).ToArray();

                double cumulativeStarLength = 0d;
                foreach (var definitionBase in sortedStars.Reverse())
                {
                    cumulativeStarLength += definitionBase.AvailableLength;
                    definitionBase.FinalLength = cumulativeStarLength;
                }

                foreach (var definitionBase in sortedStars)
                {
                    double finalLength;
                    double measureSize = definitionBase.AvailableLength;
                    if (measureSize.IsCloseTo(0))
                    {
                        finalLength = definitionBase.MinLength;
                    }
                    else
                    {
                        finalLength = (gridFinalLength - cumulativeLength).EnsurePositive();

                        finalLength *= measureSize / definitionBase.FinalLength;

                        finalLength = finalLength.Coerce(definitionBase.MinLength, definitionBase.UserMaxLength);
                    }

                    definitionBase.FinalLength = finalLength;

                    cumulativeLength += finalLength;
                }
            }

            if (cumulativeLength.IsGreaterThan(gridFinalLength))
            {
                var sortedDefinitions = starDefinitions.Concat(nonStarDefinitions).OrderBy(o => o.FinalLength - o.MinLength);

                double excessLength = cumulativeLength - gridFinalLength;
                var i = 0;
                foreach (var definitionBase in sortedDefinitions)
                {
                    double finalLength = definitionBase.FinalLength - (excessLength / (definitions.Length - i));

                    finalLength = finalLength.Coerce(definitionBase.MinLength, definitionBase.FinalLength);

                    excessLength -= definitionBase.FinalLength - finalLength;
                    definitionBase.FinalLength = finalLength;

                    i++;
                }
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
                            ColumnIndex = Math.Min(GetColumn(element), this.widthDefinitions.Length - 1), 
                            RowIndex = Math.Min(GetRow(element), this.heightDefinitions.Length - 1), 
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

        private void MeasureCell(int cellIndex, bool shouldChildBeMeasuredWithInfiniteHeight)
        {
            IElement child = this.Children[cellIndex];
            if (child != null)
            {
                Cell cell = this.cells[cellIndex];

                double x = cell.WidthType == GridUnitType.Auto
                               ? double.PositiveInfinity
                               : this.widthDefinitions[cell.ColumnIndex].AvailableLength;

                double y = cell.HeightType == GridUnitType.Auto || shouldChildBeMeasuredWithInfiniteHeight
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
                    bool shouldChildBeMeasuredWithInfiniteHeight = skipUpdateMinHeight;
                    this.MeasureCell(currentCellIndex, shouldChildBeMeasuredWithInfiniteHeight);

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
    }
}