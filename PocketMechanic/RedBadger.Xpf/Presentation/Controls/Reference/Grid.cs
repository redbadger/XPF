namespace RedBadger.Xpf.Presentation.Controls.Reference
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using RedBadger.Xpf.Internal;

    using Size = RedBadger.Xpf.Presentation.Size;

    public enum GridUnitType
    {
        Auto, 
        Pixel, 
        Star
    }

    public struct GridLength
    {
        private static readonly GridLength auto = new GridLength(1.0f, GridUnitType.Auto);

        private readonly GridUnitType unitType;

        private readonly float unitValue;

        public GridLength(float pixels)
            : this(pixels, GridUnitType.Pixel)
        {
        }

        public GridLength(float value, GridUnitType type)
        {
            if (float.IsNaN(value))
            {
                throw new ArgumentException();
            }

            if (float.IsInfinity(value))
            {
                throw new ArgumentException();
            }

            if (type != GridUnitType.Auto && type != GridUnitType.Pixel && type != GridUnitType.Star)
            {
                throw new ArgumentException();
            }

            this.unitValue = type == GridUnitType.Auto ? 0.0f : value;
            this.unitType = type;
        }

        public static GridLength Auto
        {
            get
            {
                return auto;
            }
        }

        public GridUnitType GridUnitType
        {
            get
            {
                return this.unitType;
            }
        }

        public bool IsAbsolute
        {
            get
            {
                return this.unitType == GridUnitType.Pixel;
            }
        }

        public bool IsAuto
        {
            get
            {
                return this.unitType == GridUnitType.Auto;
            }
        }

        public bool IsStar
        {
            get
            {
                return this.unitType == GridUnitType.Star;
            }
        }

        public float Value
        {
            get
            {
                return this.unitType != GridUnitType.Auto ? this.unitValue : 1.0f;
            }
        }
    }

    public class Grid : Panel
    {
        public static readonly DependencyProperty ColumnProperty = DependencyProperty.RegisterAttached(
            "Column", typeof(int), typeof(Grid), new PropertyMetadata(null));

        public static readonly DependencyProperty RowProperty = DependencyProperty.RegisterAttached(
            "Row", typeof(int), typeof(Grid), new PropertyMetadata(null));

        private static readonly StarDistributionComparerBySizeCache compareDefinitionBySizeCache =
            new StarDistributionComparerBySizeCache();

        private CellCache[] cellCachesCollection;

        private int cellGroup1;

        private int cellGroup2;

        private int cellGroup3;

        private int cellGroup4;

        private bool cellsStructureDirty;

        private bool columnDefinitionCollectionDirty;

        private ColumnDefinitionCollection columnDefinitions;

        private int[] definitionIndices;

        private DefinitionBase[] definitionsU;

        private DefinitionBase[] definitionsV;

        private bool hasGroup3CellsInAutoRows;

        private bool hasStarCellsU;

        private bool hasStarCellsV;

        private bool rowDefinitionCollectionDirty;

        private RowDefinitionCollection rowDefinitions;

        private bool sizeToContentU;

        private bool sizeToContentV;

        public ColumnDefinitionCollection ColumnDefinitions
        {
            get
            {
                return this.columnDefinitions ?? (this.columnDefinitions = new ColumnDefinitionCollection(this));
            }
        }

        public RowDefinitionCollection RowDefinitions
        {
            get
            {
                return this.rowDefinitions ?? (this.rowDefinitions = new RowDefinitionCollection(this));
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
            Size size;

            bool isSingleCell = this.columnDefinitions != null && this.rowDefinitions != null;
            if (isSingleCell)
            {
                size = new Size();
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

            bool treatStarAsAutoWidth = float.IsPositiveInfinity(availableSize.Width);
            bool treatStarAsAutoHeight = float.IsPositiveInfinity(availableSize.Height);

            if (this.rowDefinitionCollectionDirty || this.columnDefinitionCollectionDirty)
            {
                if (this.definitionIndices != null)
                {
                    Array.Clear(this.definitionIndices, 0, this.definitionIndices.Length);
                    this.definitionIndices = null;
                }
            }

            this.ValidateDefinitionsUStructure();
            this.ValidateDefinitionsLayout(this.definitionsU, treatStarAsAutoWidth);

            this.ValidateDefinitionsVStructure();
            this.ValidateDefinitionsLayout(this.definitionsV, treatStarAsAutoHeight);

            this.cellsStructureDirty |= (this.sizeToContentU != treatStarAsAutoWidth) ||
                                        (this.sizeToContentV != treatStarAsAutoHeight);
            this.sizeToContentU = treatStarAsAutoWidth;
            this.sizeToContentV = treatStarAsAutoHeight;

            this.ValidateCells();

            this.MeasureCellsGroup(this.cellGroup1, false, false);

            if (!this.hasGroup3CellsInAutoRows)
            {
                if (this.hasStarCellsV)
                {
                    ResolveStar(this.definitionsV, availableSize.Height);
                }

                this.MeasureCellsGroup(this.cellGroup2, false, false);

                if (this.hasStarCellsU)
                {
                    ResolveStar(this.definitionsU, availableSize.Width);
                }

                this.MeasureCellsGroup(this.cellGroup3, false, false);
            }
            else if (this.cellGroup2 > this.cellCachesCollection.Length)
            {
                if (this.hasStarCellsU)
                {
                    ResolveStar(this.definitionsU, availableSize.Width);
                }

                this.MeasureCellsGroup(this.cellGroup3, false, false);

                if (this.hasStarCellsV)
                {
                    ResolveStar(this.definitionsV, availableSize.Height);
                }
            }
            else
            {
                this.MeasureCellsGroup(this.cellGroup2, false, true);

                if (this.hasStarCellsU)
                {
                    ResolveStar(this.definitionsU, availableSize.Width);
                }

                this.MeasureCellsGroup(this.cellGroup3, false, false);

                if (this.hasStarCellsV)
                {
                    ResolveStar(this.definitionsV, availableSize.Height);
                }

                this.MeasureCellsGroup(this.cellGroup2, true, false);
            }

            this.MeasureCellsGroup(this.cellGroup4, false, false);
            size = new Size(this.definitionsU.Sum(t => t.MinSize), this.definitionsV.Sum(t => t.MinSize));

            return size;
        }

        private static bool CompareNullRefs(object x, object y, out int result)
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

        private static void ResolveStar(IEnumerable<DefinitionBase> definitions, float availableSize)
        {
            float occupiedSpace = 0.0f;

            var stars = new List<DefinitionBase>();

            foreach (DefinitionBase definition in definitions)
            {
                switch (definition.SizeType)
                {
                    case GridUnitType.Pixel:
                        occupiedSpace += definition.MeasureSize;
                        break;

                    case GridUnitType.Auto:
                        occupiedSpace += definition.MinSize;
                        break;

                    case GridUnitType.Star:
                        stars.Add(definition);
                        float divisor = definition.UserSize.Value;
                        if (divisor.IsCloseTo(0f))
                        {
                            definition.MeasureSize = 0.0f;
                            definition.SizeCache = 0.0f;
                        }
                        else
                        {
                            definition.MeasureSize = divisor;
                            definition.SizeCache = Math.Max(definition.MinSize, definition.UserMaxSize) / divisor;
                        }

                        break;
                }
            }

            if (stars.Count > 0)
            {
                stars.Sort(compareDefinitionBySizeCache);

                float cumulativeMeasureSize = 0.0f;
                foreach (DefinitionBase definition in Enumerable.Reverse(stars))
                {
                    cumulativeMeasureSize += definition.MeasureSize;
                    definition.SizeCache = cumulativeMeasureSize;
                }

                foreach (DefinitionBase definition in stars)
                {
                    float size;
                    float divisor = definition.MeasureSize;
                    if (divisor.IsCloseTo(0f))
                    {
                        size = definition.MinSize;
                    }
                    else
                    {
                        size = Math.Max(availableSize - occupiedSpace, 0.0f) * (divisor / definition.SizeCache);
                        size = Math.Max(definition.MinSize, Math.Min(size, definition.UserMaxSize));
                    }

                    definition.MeasureSize = size;
                    occupiedSpace += size;
                }
            }
        }

        private void MeasureCell(int cellIndex, bool forceInfinityV)
        {
            UIElement child = this.Children[cellIndex];
            if (child != null)
            {
                CellCache cell = this.cellCachesCollection[cellIndex];

                float x = cell.SizeTypeU == GridUnitType.Auto
                              ? float.PositiveInfinity
                              : this.definitionsU[cell.ColumnIndex].MeasureSize;

                float y = cell.SizeTypeV == GridUnitType.Auto || forceInfinityV
                              ? float.PositiveInfinity
                              : this.definitionsV[cell.RowIndex].MeasureSize;

                child.Measure(new Size(x, y));
            }
        }

        private void MeasureCellsGroup(int headCellIndex, bool ignoreDesiredSizeWidth, bool forceInfinityHeight)
        {
            if (headCellIndex < this.cellCachesCollection.Length)
            {
                int currentCellIndex = headCellIndex;

                do
                {
                    this.MeasureCell(currentCellIndex, forceInfinityHeight);

                    if (!ignoreDesiredSizeWidth)
                    {
                        this.definitionsU[this.cellCachesCollection[currentCellIndex].ColumnIndex].UpdateMinSize(
                            Math.Min(
                                this.Children[currentCellIndex].DesiredSize.Width, 
                                this.definitionsU[this.cellCachesCollection[currentCellIndex].ColumnIndex].UserMaxSize));
                    }

                    if (!forceInfinityHeight)
                    {
                        this.definitionsV[this.cellCachesCollection[currentCellIndex].RowIndex].UpdateMinSize(
                            Math.Min(
                                this.Children[currentCellIndex].DesiredSize.Height, 
                                this.definitionsV[this.cellCachesCollection[currentCellIndex].RowIndex].UserMaxSize));
                    }

                    currentCellIndex = this.cellCachesCollection[currentCellIndex].Next;
                }
                while (currentCellIndex < this.cellCachesCollection.Length);
            }
        }

        private void ValidateCells()
        {
            if (this.cellsStructureDirty)
            {
                this.ValidateCellsCore();
                this.cellsStructureDirty = false;
            }
        }

        private void ValidateCellsCore()
        {
            UIElementCollection children = this.Children;
            this.cellCachesCollection = new CellCache[children.Count];
            this.cellGroup1 = int.MaxValue;
            this.cellGroup2 = int.MaxValue;
            this.cellGroup3 = int.MaxValue;
            this.cellGroup4 = int.MaxValue;
            bool hasStarCellsU = false;
            bool hasStarCellsV = false;
            bool hasGroup3CellsInAutoRows = false;
            for (int i = this.cellCachesCollection.Length - 1; i >= 0; i--)
            {
                UIElement element = children[i];
                if (element != null)
                {
                    var cache = new CellCache
                        {
                            ColumnIndex = Math.Min(GetColumn(element), this.definitionsU.Length - 1), 
                            RowIndex = Math.Min(GetRow(element), this.definitionsV.Length - 1)
                        };
                    cache.SizeTypeU = this.definitionsU[cache.ColumnIndex].SizeType;
                    cache.SizeTypeV = this.definitionsV[cache.RowIndex].SizeType;
                    hasStarCellsU |= cache.SizeTypeU == GridUnitType.Star;
                    hasStarCellsV |= cache.SizeTypeV == GridUnitType.Star;

                    if (cache.SizeTypeV != GridUnitType.Star)
                    {
                        if (cache.SizeTypeU != GridUnitType.Star)
                        {
                            cache.Next = this.cellGroup1;
                            this.cellGroup1 = i;
                        }
                        else
                        {
                            cache.Next = this.cellGroup3;
                            this.cellGroup3 = i;
                            hasGroup3CellsInAutoRows |= cache.SizeTypeV == GridUnitType.Auto;
                        }
                    }
                    else if (cache.SizeTypeU == GridUnitType.Auto && cache.SizeTypeU != GridUnitType.Star)
                    {
                        cache.Next = this.cellGroup2;
                        this.cellGroup2 = i;
                    }
                    else
                    {
                        cache.Next = this.cellGroup4;
                        this.cellGroup4 = i;
                    }

                    this.cellCachesCollection[i] = cache;
                }
            }

            this.hasStarCellsU = hasStarCellsU;
            this.hasStarCellsV = hasStarCellsV;
            this.hasGroup3CellsInAutoRows = hasGroup3CellsInAutoRows;
        }

        private void ValidateDefinitionsLayout(IEnumerable<DefinitionBase> definitions, bool treatStarAsAuto)
        {
            foreach (DefinitionBase definition in definitions)
            {
                definition.OnBeforeLayout(this);
                float userMinSize = definition.UserMinSize;
                float userMaxSize = definition.UserMaxSize;
                float size = 0.0f;

                switch (definition.UserSize.GridUnitType)
                {
                    case GridUnitType.Auto:
                        definition.SizeType = GridUnitType.Auto;
                        size = float.PositiveInfinity;
                        break;

                    case GridUnitType.Pixel:
                        definition.SizeType = GridUnitType.Pixel;
                        size = definition.UserSize.Value;
                        userMinSize = Math.Max(userMinSize, Math.Min(size, userMaxSize));
                        break;

                    case GridUnitType.Star:
                        definition.SizeType = treatStarAsAuto ? GridUnitType.Auto : GridUnitType.Star;
                        size = float.PositiveInfinity;
                        break;
                }

                definition.UpdateMinSize(userMinSize);
                definition.MeasureSize = Math.Max(userMinSize, Math.Min(size, userMaxSize));
            }
        }

        private void ValidateDefinitionsUStructure()
        {
            if (this.columnDefinitionCollectionDirty)
            {
                if (this.columnDefinitions == null)
                {
                    if (this.definitionsU == null)
                    {
                        this.definitionsU = new[] { new ColumnDefinition() };
                    }
                }
                else
                {
                    this.columnDefinitions.InternalTrimToSize();
                    this.definitionsU = this.columnDefinitions.InternalCount == 0
                                            ? new[] { new ColumnDefinition() }
                                            : this.columnDefinitions.InternalItems;
                }

                this.columnDefinitionCollectionDirty = false;
            }
        }

        private void ValidateDefinitionsVStructure()
        {
            if (this.rowDefinitionCollectionDirty)
            {
                if (this.rowDefinitions == null)
                {
                    if (this.definitionsV == null)
                    {
                        this.definitionsV = new[] { new RowDefinition() };
                    }
                }
                else
                {
                    this.rowDefinitions.InternalTrimToSize();
                    this.definitionsV = this.rowDefinitions.InternalCount == 0
                                            ? new[] { new RowDefinition() }
                                            : this.rowDefinitions.InternalItems;
                }

                this.rowDefinitionCollectionDirty = false;
            }
        }

        private struct CellCache
        {
            internal int ColumnIndex;

            internal int Next;

            internal int RowIndex;

            internal GridUnitType SizeTypeU;

            internal GridUnitType SizeTypeV;
        }

        private class StarDistributionComparerBySizeCache : IComparer<DefinitionBase>
        {
            public int Compare(DefinitionBase x, DefinitionBase y)
            {
                int num;
                if (!CompareNullRefs(x, y, out num))
                {
                    num = x.SizeCache.CompareTo(y.SizeCache);
                }

                return num;
            }
        }
    }

    internal class RowDefinition : DefinitionBase
    {
    }

    public class RowDefinitionCollection
    {
        public RowDefinitionCollection(Grid grid)
        {
        }

        public int InternalCount { get; set; }

        public DefinitionBase[] InternalItems { get; set; }

        public void InternalTrimToSize()
        {
        }
    }

    public class ColumnDefinitionCollection
    {
        public ColumnDefinitionCollection(Grid grid)
        {
        }

        public int InternalCount { get; set; }

        public DefinitionBase[] InternalItems { get; set; }

        public void InternalTrimToSize()
        {
        }
    }

    internal class ColumnDefinition : DefinitionBase
    {
    }

    public class DefinitionBase
    {
        public float MeasureSize { get; set; }

        public float MinSize { get; set; }

        public float SizeCache { get; set; }

        public GridUnitType SizeType { get; set; }

        public float UserMaxSize { get; set; }

        public float UserMinSize { get; set; }

        public GridLength UserSize { get; set; }

        public void OnBeforeLayout(Grid grid)
        {
        }

        public void UpdateMinSize(float min)
        {
        }
    }
}