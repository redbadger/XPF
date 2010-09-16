namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RedBadger.Xpf.Internal;

    /// <summary>
    ///     A Grid layout panel consisting of columns and rows.
    /// </summary>
    public class Grid : Panel
    {
        /// <summary>
        ///     Column attached property.
        /// </summary>
        public static readonly ReactiveProperty<int, Grid> ColumnProperty =
            ReactiveProperty<int, Grid>.Register("Column");

        /// <summary>
        ///     Row attached property.
        /// </summary>
        public static readonly ReactiveProperty<int, Grid> RowProperty = ReactiveProperty<int, Grid>.Register("Row");

        private readonly LinkedList<Cell> cellsWithAllStars = new LinkedList<Cell>();

        private readonly LinkedList<Cell> cellsWithNoStars = new LinkedList<Cell>();

        private readonly LinkedList<Cell> cellsWithoutStarHeights = new LinkedList<Cell>();

        private readonly LinkedList<Cell> cellsWithoutStarWidths = new LinkedList<Cell>();

        private readonly IList<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();

        private readonly IList<RowDefinition> rowDefinitions = new List<RowDefinition>();

        private bool areThereAnyStarHeights;

        private bool areThereAnyStarWidths;

        private Cell[] cells;

        private DefinitionBase[] heightDefinitions;

        private bool noAutoHeightStarWidthCells;

        private DefinitionBase[] widthDefinitions;

        private enum UpdateMinLengths
        {
            SkipHeights, 
            SkipWidths, 
            WidthsAndHeights
        }

        /// <summary>
        ///     Gets the collection of column definitions.
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
        ///     Gets the collection of row definitions.
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
        ///     Gets the value of the Column attached property for the specified element.
        /// </summary>
        /// <param name = "element">The element for which to read the proerty value.</param>
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
        ///     Gets the value of the Row attached property for the specified element.
        /// </summary>
        /// <param name = "element">The element for which to read the proerty value.</param>
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
        ///     Sets the value of the Column attached property for the specified element.
        /// </summary>
        /// <param name = "element">The element for which to write the proerty value.</param>
        /// <param name = "value">The value of the Column attached property.</param>
        public static void SetColumn(IElement element, int value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(ColumnProperty, value);
        }

        /// <summary>
        ///     Sets the value of the Row attached property for the specified element.
        /// </summary>
        /// <param name = "element">The element for which to write the proerty value.</param>
        /// <param name = "value">The value of the Row attached property.</param>
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

            this.MeasureCellGroup(this.cellsWithNoStars, UpdateMinLengths.WidthsAndHeights);

            if (this.noAutoHeightStarWidthCells)
            {
                if (this.areThereAnyStarHeights)
                {
                    ResolveStar(this.heightDefinitions, availableSize.Height);
                }

                this.MeasureCellGroup(this.cellsWithoutStarWidths, UpdateMinLengths.WidthsAndHeights);

                if (this.areThereAnyStarWidths)
                {
                    ResolveStar(this.widthDefinitions, availableSize.Width);
                }

                this.MeasureCellGroup(this.cellsWithoutStarHeights, UpdateMinLengths.WidthsAndHeights);
            }
            else
            {
                if (this.cellsWithoutStarWidths.Count == 0)
                {
                    if (this.areThereAnyStarWidths)
                    {
                        ResolveStar(this.widthDefinitions, availableSize.Width);
                    }

                    this.MeasureCellGroup(this.cellsWithoutStarHeights, UpdateMinLengths.WidthsAndHeights);

                    if (this.areThereAnyStarHeights)
                    {
                        ResolveStar(this.heightDefinitions, availableSize.Height);
                    }
                }
                else
                {
                    this.MeasureCellGroup(this.cellsWithoutStarWidths, UpdateMinLengths.SkipHeights);

                    if (this.areThereAnyStarWidths)
                    {
                        ResolveStar(this.widthDefinitions, availableSize.Width);
                    }

                    this.MeasureCellGroup(this.cellsWithoutStarHeights, UpdateMinLengths.WidthsAndHeights);

                    if (this.areThereAnyStarHeights)
                    {
                        ResolveStar(this.heightDefinitions, availableSize.Height);
                    }

                    this.MeasureCellGroup(this.cellsWithoutStarWidths, UpdateMinLengths.SkipWidths);
                }
            }

            this.MeasureCellGroup(this.cellsWithAllStars, UpdateMinLengths.WidthsAndHeights);

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
                DefinitionBase[] sortedStars = stars.OrderBy(o => o.FinalLength).ToArray();

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
                DefinitionBase[] sortedStars = starDefinitions.OrderBy(o => o.FinalLength).ToArray();

                double cumulativeStarLength = 0d;
                foreach (DefinitionBase definitionBase in sortedStars.Reverse())
                {
                    cumulativeStarLength += definitionBase.AvailableLength;
                    definitionBase.FinalLength = cumulativeStarLength;
                }

                foreach (DefinitionBase definitionBase in sortedStars)
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
                IOrderedEnumerable<DefinitionBase> sortedDefinitions =
                    starDefinitions.Concat(nonStarDefinitions).OrderBy(o => o.FinalLength - o.MinLength);

                double excessLength = cumulativeLength - gridFinalLength;
                int i = 0;
                foreach (DefinitionBase definitionBase in sortedDefinitions)
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
            this.cellsWithNoStars.Clear();
            this.cellsWithoutStarHeights.Clear();
            this.cellsWithoutStarWidths.Clear();
            this.cellsWithAllStars.Clear();

            this.areThereAnyStarWidths = false;
            this.areThereAnyStarHeights = false;
            bool doAnyCellsHaveAutoHeightAndStarWidth = false;

            for (int i = this.cells.Length - 1; i >= 0; i--)
            {
                IElement element = this.Children[i];
                if (element != null)
                {
                    var columnIndex = Math.Min(GetColumn(element), this.widthDefinitions.Length - 1);
                    var rowIndex = Math.Min(GetRow(element), this.heightDefinitions.Length - 1);
                    var cell = new Cell
                        {
                            ColumnIndex = columnIndex,
                            RowIndex = rowIndex,
                            Child = element,
                            WidthType = this.widthDefinitions[columnIndex].LengthType,
                            HeightType = this.heightDefinitions[rowIndex].LengthType,
                        };

                    this.areThereAnyStarWidths |= cell.WidthType == GridUnitType.Star;
                    this.areThereAnyStarHeights |= cell.HeightType == GridUnitType.Star;

                    if (cell.HeightType != GridUnitType.Star)
                    {
                        if (cell.WidthType != GridUnitType.Star)
                        {
                            this.cellsWithNoStars.AddLast(cell);
                        }
                        else
                        {
                            this.cellsWithoutStarHeights.AddLast(cell);

                            doAnyCellsHaveAutoHeightAndStarWidth |= cell.HeightType == GridUnitType.Auto;
                        }
                    }
                    else if (cell.WidthType != GridUnitType.Star)
                    {
                        this.cellsWithoutStarWidths.AddLast(cell);
                    }
                    else
                    {
                        this.cellsWithAllStars.AddLast(cell);
                    }

                    this.cells[i] = cell;
                }
            }

            this.noAutoHeightStarWidthCells = !doAnyCellsHaveAutoHeightAndStarWidth;
        }

        private void MeasureCell(Cell cell, IElement child, bool shouldChildBeMeasuredWithInfiniteHeight)
        {
            if (child != null)
            {
                double x = cell.WidthType == GridUnitType.Auto
                               ? double.PositiveInfinity
                               : this.widthDefinitions[cell.ColumnIndex].AvailableLength;

                double y = cell.HeightType == GridUnitType.Auto || shouldChildBeMeasuredWithInfiniteHeight
                               ? double.PositiveInfinity
                               : this.heightDefinitions[cell.RowIndex].AvailableLength;

                child.Measure(new Size(x, y));
            }
        }

        private void MeasureCellGroup(IEnumerable<Cell> cells, UpdateMinLengths updateMinLengths)
        {
            foreach (Cell cell in cells)
            {
                bool shouldChildBeMeasuredWithInfiniteHeight = updateMinLengths == UpdateMinLengths.SkipHeights;

                this.MeasureCell(cell, cell.Child, shouldChildBeMeasuredWithInfiniteHeight);

                if (updateMinLengths != UpdateMinLengths.SkipWidths)
                {
                    DefinitionBase widthDefinition = this.widthDefinitions[cell.ColumnIndex];
                    widthDefinition.UpdateMinLength(
                        Math.Min(cell.Child.DesiredSize.Width, widthDefinition.UserMaxLength));
                }

                if (updateMinLengths != UpdateMinLengths.SkipHeights)
                {
                    DefinitionBase heightDefinition = this.heightDefinitions[cell.RowIndex];
                    heightDefinition.UpdateMinLength(
                        Math.Min(cell.Child.DesiredSize.Height, heightDefinition.UserMaxLength));
                }
            }
        }

        private struct Cell
        {
            public IElement Child;

            public int ColumnIndex;

            public GridUnitType HeightType;

            public int RowIndex;

            public GridUnitType WidthType;
        }
    }
}