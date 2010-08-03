namespace RedBadger.Xpf.Presentation.Controls.Reference
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using RedBadger.Xpf.Internal;

    using Rect = RedBadger.Xpf.Presentation.Rect;
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

        private CellCache[] cellCaches;

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

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            bool isSingleCell = this.columnDefinitions != null && this.rowDefinitions != null;
            if (isSingleCell)
            {
                foreach (UIElement child in this.Children)
                {
                    child.Arrange(new Rect(arrangeSize));
                }

                return arrangeSize;
            }

            this.SetFinalSize(this.definitionsU, arrangeSize.Width);
            this.SetFinalSize(this.definitionsV, arrangeSize.Height);

            for (int i = 0; i < this.cellCaches.Length; i++)
            {
                UIElement child = this.Children[i];
                if (child != null)
                {
                    int columnIndex = this.cellCaches[i].ColumnIndex;
                    int rowIndex = this.cellCaches[i].RowIndex;
                    var finalRect = new Rect(
                        (columnIndex == 0) ? 0.0f : this.definitionsU[columnIndex].FinalOffset, 
                        (rowIndex == 0) ? 0.0f : this.definitionsV[rowIndex].FinalOffset, 
                        this.definitionsU[columnIndex].SizeCache, 
                        this.definitionsV[rowIndex].SizeCache);
                    child.Arrange(finalRect);
                }
            }

            return arrangeSize;
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
            else if (this.cellGroup2 > this.cellCaches.Length)
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
                CellCache cell = this.cellCaches[cellIndex];

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
            if (headCellIndex < this.cellCaches.Length)
            {
                int currentCellIndex = headCellIndex;

                do
                {
                    this.MeasureCell(currentCellIndex, forceInfinityHeight);

                    if (!ignoreDesiredSizeWidth)
                    {
                        this.definitionsU[this.cellCaches[currentCellIndex].ColumnIndex].UpdateMinSize(
                            Math.Min(
                                this.Children[currentCellIndex].DesiredSize.Width, 
                                this.definitionsU[this.cellCaches[currentCellIndex].ColumnIndex].UserMaxSize));
                    }

                    if (!forceInfinityHeight)
                    {
                        this.definitionsV[this.cellCaches[currentCellIndex].RowIndex].UpdateMinSize(
                            Math.Min(
                                this.Children[currentCellIndex].DesiredSize.Height, 
                                this.definitionsV[this.cellCaches[currentCellIndex].RowIndex].UserMaxSize));
                    }

                    currentCellIndex = this.cellCaches[currentCellIndex].Next;
                }
                while (currentCellIndex < this.cellCaches.Length);
            }
        }

        private void SetFinalSize(DefinitionBase[] definitions, float finalSize)
        {
            int length = 0;
            int num2 = definitions.Length;
            float num3 = 0.0f;

            for (int i = 0; i < definitions.Length; i++)
            {
                if (definitions[i].UserSize.GridUnitType == GridUnitType.Star)
                {
                    float divisor = definitions[i].UserSize.Value;
                    if (divisor.IsCloseTo(0f))
                    {
                        definitions[i].MeasureSize = 0.0f;
                        definitions[i].SizeCache = 0.0f;
                    }
                    else
                    {
                        definitions[i].MeasureSize = divisor;
                        definitions[i].SizeCache =
                            Math.Max(definitions[i].MinSizeForArrange, definitions[i].UserMaxSize) / divisor;
                    }

                    this.definitionIndices[length++] = i;
                    continue;
                }

                float minSizeForArrange = 0.0f;
                switch (definitions[i].UserSize.GridUnitType)
                {
                    case GridUnitType.Auto:
                        minSizeForArrange = definitions[i].MinSizeForArrange;
                        break;

                    case GridUnitType.Pixel:
                        minSizeForArrange = definitions[i].UserSize.Value;
                        break;
                }

                float userMaxSize = definitions[i].IsShared ? minSizeForArrange : definitions[i].UserMaxSize;

                definitions[i].SizeCache = Math.Max(
                    definitions[i].MinSizeForArrange, Math.Min(minSizeForArrange, userMaxSize));

                num3 += definitions[i].SizeCache;
                this.definitionIndices[--num2] = i;
            }

            if (length > 0)
            {
                Array.Sort(this.definitionIndices, 0, length, new StarDistributionOrderIndexComparer(definitions));
                float num10 = 0.0f;
                int index = length - 1;
                do
                {
                    num10 += definitions[this.definitionIndices[index]].MeasureSize;
                    definitions[this.definitionIndices[index]].SizeCache = num10;
                }
                while (--index >= 0);
                index = 0;
                do
                {
                    float num12;
                    float measureSize = definitions[this.definitionIndices[index]].MeasureSize;
                    if (measureSize.IsCloseTo(0f))
                    {
                        num12 = definitions[this.definitionIndices[index]].MinSizeForArrange;
                    }
                    else
                    {
                        float num14 = Math.Max(finalSize - num3, 0.0f) *
                                      (measureSize / definitions[this.definitionIndices[index]].SizeCache);
                        num12 = Math.Min(num14, definitions[this.definitionIndices[index]].UserMaxSize);
                        num12 = Math.Max(definitions[this.definitionIndices[index]].MinSizeForArrange, num12);
                    }

                    definitions[this.definitionIndices[index]].SizeCache = num12;

                    num3 += definitions[this.definitionIndices[index]].SizeCache;
                }
                while (++index < length);
            }

            if (num3.IsGreaterThan(finalSize))
            {
                Array.Sort(this.definitionIndices, 0, definitions.Length, new DistributionOrderIndexComparer(definitions));
                float num15 = finalSize - num3;
                for (int k = 0; k < definitions.Length; k++)
                {
                    int num17 = this.definitionIndices[k];
                    float num18 = definitions[num17].SizeCache + (num15 / (definitions.Length - k));
                    num18 = Math.Min(
                        Math.Max(num18, definitions[num17].MinSizeForArrange), definitions[num17].SizeCache);

                    num15 -= num18 - definitions[num17].SizeCache;
                    definitions[num17].SizeCache = num18;
                }
            }

            definitions[0].FinalOffset = 0.0f;
            for (int j = 0; j < definitions.Length; j++)
            {
                definitions[(j + 1) % definitions.Length].FinalOffset = definitions[j].FinalOffset +
                                                                        definitions[j].SizeCache;
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

        /// <summary>
        /// CreateCells
        /// </summary>
        private void ValidateCellsCore()
        {
            IList<UIElement> children = this.Children;
            this.cellCaches = new CellCache[children.Count];
            this.cellGroup1 = int.MaxValue;
            this.cellGroup2 = int.MaxValue;
            this.cellGroup3 = int.MaxValue;
            this.cellGroup4 = int.MaxValue;
            bool hasStarCellsU = false;
            bool hasStarCellsV = false;
            bool hasGroup3CellsInAutoRows = false;
            for (int i = this.cellCaches.Length - 1; i >= 0; i--)
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

                    this.cellCaches[i] = cache;
                }
            }

            this.hasStarCellsU = hasStarCellsU;
            this.hasStarCellsV = hasStarCellsV;
            this.hasGroup3CellsInAutoRows = hasGroup3CellsInAutoRows;
        }

        /// <summary>
        /// InitializeMeasureData
        /// </summary>
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

    internal class StarDistributionOrderIndexComparer : IComparer<int>
    {
        private readonly DefinitionBase[] definitions;

        internal StarDistributionOrderIndexComparer(DefinitionBase[] definitions)
        {
            if (definitions == null)
            {
                throw new ArgumentNullException("definitions");
            }

            this.definitions = definitions;
        }

        public int Compare(int x, int y)
        {
            int num;
            DefinitionBase left = this.definitions[x];
            DefinitionBase right = this.definitions[y];

            if (!Grid.CompareNullRefs(left, right, out num))
            {
                num = left.SizeCache.CompareTo(right.SizeCache);
            }

            return num;
        }
    }

    internal class DistributionOrderIndexComparer : IComparer<int>
    {
        private readonly DefinitionBase[] definitions;

        internal DistributionOrderIndexComparer(DefinitionBase[] definitions)
        {
            if (definitions == null)
            {
                throw new ArgumentNullException("definitions");
            }

            this.definitions = definitions;
        }

        public int Compare(int x, int y)
        {
            int num;
            DefinitionBase left = this.definitions[x];
            DefinitionBase right = this.definitions[y];
            if (!Grid.CompareNullRefs(left, right, out num))
            {
                double num2 = left.SizeCache - left.MinSizeForArrange;
                double num3 = right.SizeCache - right.MinSizeForArrange;
                num = num2.CompareTo(num3);
            }

            return num;
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
        public float FinalOffset { get; set; }

        public bool IsShared { get; set; }

        public float MeasureSize { get; set; }

        public float MinSize { get; set; }

        public float SizeCache { get; set; }

        public GridUnitType SizeType { get; set; }

        public float UserMaxSize { get; set; }

        public float UserMinSize { get; set; }

        public GridLength UserSize { get; set; }

        internal float MinSizeForArrange
        {
            get
            {
                /*
                float minSize = this._minSize;
                if (((this._sharedState != null) && (this.UseSharedMinimum || !this.LayoutWasUpdated)) && (minSize < this._sharedState.MinSize))
                {
                    minSize = this._sharedState.MinSize;
                }

                return minSize;
*/
                return this.MinSize;
            }
        }

        public void OnBeforeLayout(Grid grid)
        {
        }

        public void UpdateMinSize(float min)
        {
        }
    }
}