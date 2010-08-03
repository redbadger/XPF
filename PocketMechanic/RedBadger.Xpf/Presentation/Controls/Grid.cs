namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using Size = RedBadger.Xpf.Presentation.Size;

#if WINDOWS_PHONE
    using UIElement = RedBadger.Xpf.Presentation.UIElement;
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

        private int cellsWithoutStarsHeadIndex;

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
            this.MeasureCellGroup(this.cellsWithoutStarsHeadIndex);

            return new Size(
                this.widthDefinitions.Sum(definition => definition.MinSize),
                this.heightDefinitions.Sum(definition => definition.MinSize));
        }

        private static void InitializeMeasureData(IEnumerable<DefinitionBase> definitions)
        {
            foreach (DefinitionBase definition in definitions)
            {
                definition.AvailableSize = float.PositiveInfinity;
            }
        }

        private void CreateCells()
        {
            this.cells = new Cell[this.Children.Count];
            this.cellsWithoutStarsHeadIndex = int.MaxValue;

            for (int i = this.cells.Length - 1; i >= 0; i--)
            {
                UIElement element = this.Children[i];
                if (element != null)
                {
                    var cell = new Cell
                        {
                            ColumnIndex = GetColumn(element), 
                            RowIndex = GetRow(element), 
                            Next = this.cellsWithoutStarsHeadIndex
                        };

                    this.cellsWithoutStarsHeadIndex = i;

                    this.cells[i] = cell;
                }
            }
        }

        private void MeasureCell(int cellIndex)
        {
            UIElement child = this.Children[cellIndex];
            if (child != null)
            {
                child.Measure(new Size(float.PositiveInfinity, float.PositiveInfinity));
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

                    this.widthDefinitions[this.cells[currentCellIndex].ColumnIndex].UpdateMinSize(
                        this.Children[currentCellIndex].DesiredSize.Width);

                    this.heightDefinitions[this.cells[currentCellIndex].RowIndex].UpdateMinSize(
                        this.Children[currentCellIndex].DesiredSize.Height);

                    currentCellIndex = this.cells[currentCellIndex].Next;
                }
                while (currentCellIndex < this.cells.Length);
            }
        }

        private class Cell
        {
            public int ColumnIndex { get; set; }

            public int Next { get; set; }

            public int RowIndex { get; set; }
        }
    }
}