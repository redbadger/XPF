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
            Size size = Size.Empty;

            bool isSingleCell = this.columnDefinitions.Count == 0;
            if (isSingleCell)
            {
                foreach (UIElement child in this.Children)
                {
                    if (child != null)
                    {
                        child.Measure(availableSize);
                        size.Width = Math.Max(size.Width, child.DesiredSize.Width);
                        size.Height = Math.Max(size.Height, child.DesiredSize.Height);
                    }
                }

                return size;
            }

            InitializeMeasureData((IEnumerable<DefinitionBase>)this.columnDefinitions);
            InitializeMeasureData((IEnumerable<DefinitionBase>)this.rowDefinitions);
            this.CreateCells();
            this.MeasureCellGroup(this.cellsWithoutStarsHeadIndex);

            size = new Size(this.columnDefinitions.Sum(definition => definition.MinSize), availableSize.Height);

            return size;
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
                    var cell = new Cell { ColumnIndex = GetColumn(element), Next = this.cellsWithoutStarsHeadIndex };

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

                    this.columnDefinitions[this.cells[currentCellIndex].ColumnIndex].UpdateMinSize(
                        this.Children[currentCellIndex].DesiredSize.Width);

                    currentCellIndex = this.cells[currentCellIndex].Next;
                }
                while (currentCellIndex < this.cells.Length);
            }
        }

        private class Cell
        {
            public int ColumnIndex { get; set; }

            public int Next { get; set; }
        }
    }
}