namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Collections;

    public class Grid : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            Size size;
            ExtendedData extData = this.ExtData;

            try
            {
                this.ListenToNotifications = true;
                this.MeasureOverrideInProgress = true;
                
                if (extData == null)
                {
                    size = new Size();
                    UIElementCollection internalChildren = this.Children;
                    int num = 0;
                    int count = internalChildren.Count;

                    while (num < count)
                    {
                        UIElement child = internalChildren[num];
                        if (child != null)
                        {
                            Helper.SetMeasureDataOnChild(this, child, availableSize);
                            child.Measure(availableSize);
                            size.Width = Math.Max(size.Width, child.DesiredSize.Width);
                            size.Height = Math.Max(size.Height, child.DesiredSize.Height);
                        }

                        num++;
                    }

                    return size;
                }

                // if extData != null
                bool treatStarAsAutoWidth = double.IsPositiveInfinity(availableSize.Width);
                bool treatStarAsAutoHeight = double.IsPositiveInfinity(availableSize.Height);

                if (this.RowDefinitionCollectionDirty || this.ColumnDefinitionCollectionDirty)
                {
                    if (this._definitionIndices != null)
                    {
                        Array.Clear(this._definitionIndices, 0, this._definitionIndices.Length);
                        this._definitionIndices = null;
                    }

                    if (base.UseLayoutRounding && (this._roundingErrors != null))
                    {
                        Array.Clear(this._roundingErrors, 0, this._roundingErrors.Length);
                        this._roundingErrors = null;
                    }
                }

                this.ValidateDefinitionsUStructure();
                this.ValidateDefinitionsLayout(this.DefinitionsU, treatStarAsAutoWidth);
                
                this.ValidateDefinitionsVStructure();
                this.ValidateDefinitionsLayout(this.DefinitionsV, treatStarAsAutoHeight);

                this.CellsStructureDirty |= (this.SizeToContentU != treatStarAsAutoWidth) || (this.SizeToContentV != treatStarAsAutoHeight);
                this.SizeToContentU = treatStarAsAutoWidth;
                this.SizeToContentV = treatStarAsAutoHeight;

                this.ValidateCells();

                this.MeasureCellsGroup(extData.CellGroup1, availableSize, false, false);
                if (!this.HasGroup3CellsInAutoRows)
                {
                    if (this.HasStarCellsV)
                    {
                        this.ResolveStar(this.DefinitionsV, availableSize.Height);
                    }

                    this.MeasureCellsGroup(extData.CellGroup2, availableSize, false, false);
                    
                    if (this.HasStarCellsU)
                    {
                        this.ResolveStar(this.DefinitionsU, availableSize.Width);
                    }

                    this.MeasureCellsGroup(extData.CellGroup3, availableSize, false, false);
                }
                else if (extData.CellGroup2 > this.PrivateCells.Length)
                {
                    if (this.HasStarCellsU)
                    {
                        this.ResolveStar(this.DefinitionsU, availableSize.Width);
                    }

                    this.MeasureCellsGroup(extData.CellGroup3, availableSize, false, false);
                    if (this.HasStarCellsV)
                    {
                        this.ResolveStar(this.DefinitionsV, availableSize.Height);
                    }
                }
                else
                {
                    this.MeasureCellsGroup(extData.CellGroup2, availableSize, false, true);
                    if (this.HasStarCellsU)
                    {
                        this.ResolveStar(this.DefinitionsU, availableSize.Width);
                    }

                    this.MeasureCellsGroup(extData.CellGroup3, availableSize, false, false);
                    if (this.HasStarCellsV)
                    {
                        this.ResolveStar(this.DefinitionsV, availableSize.Height);
                    }

                    this.MeasureCellsGroup(extData.CellGroup2, availableSize, true, false);
                }

                this.MeasureCellsGroup(extData.CellGroup4, availableSize, false, false);
                size = new Size(this.CalculateDesiredSize(this.DefinitionsU), this.CalculateDesiredSize(this.DefinitionsV));
            }
            finally
            {
                this.MeasureOverrideInProgress = false;
            }

            return size;
        }

        private void ValidateDefinitionsUStructure()
        {
            if (this.ColumnDefinitionCollectionDirty)
            {
                ExtendedData extData = this.ExtData;
                if (extData.ColumnDefinitions == null)
                {
                    if (extData.DefinitionsU == null)
                    {
                        extData.DefinitionsU = new DefinitionBase[] { new ColumnDefinition() };
                    }
                }
                else
                {
                    extData.ColumnDefinitions.InternalTrimToSize();
                    if (extData.ColumnDefinitions.InternalCount == 0)
                    {
                        extData.DefinitionsU = new DefinitionBase[] { new ColumnDefinition() };
                    }
                    else
                    {
                        extData.DefinitionsU = extData.ColumnDefinitions.InternalItems;
                    }
                }
                this.ColumnDefinitionCollectionDirty = false;
            }
        }

        private void ValidateDefinitionsVStructure()
        {
            if (this.RowDefinitionCollectionDirty)
            {
                ExtendedData extData = this.ExtData;
                if (extData.RowDefinitions == null)
                {
                    if (extData.DefinitionsV == null)
                    {
                        extData.DefinitionsV = new DefinitionBase[] { new RowDefinition() };
                    }
                }
                else
                {
                    extData.RowDefinitions.InternalTrimToSize();
                    if (extData.RowDefinitions.InternalCount == 0)
                    {
                        extData.DefinitionsV = new DefinitionBase[] { new RowDefinition() };
                    }
                    else
                    {
                        extData.DefinitionsV = extData.RowDefinitions.InternalItems;
                    }
                }
                this.RowDefinitionCollectionDirty = false;
            }
        }

        private void ValidateDefinitionsLayout(DefinitionBase[] definitions, bool treatStarAsAuto)
        {
            for (int i = 0; i < definitions.Length; i++)
            {
                definitions[i].OnBeforeLayout(this);
                double userMinSize = definitions[i].UserMinSize;
                double userMaxSize = definitions[i].UserMaxSize;
                double positiveInfinity = 0.0;

                switch (definitions[i].UserSize.GridUnitType)
                {
                    case GridUnitType.Auto:
                        definitions[i].SizeType = LayoutTimeSizeType.Auto;
                        positiveInfinity = double.PositiveInfinity;
                        break;

                    case GridUnitType.Pixel:
                        definitions[i].SizeType = LayoutTimeSizeType.Pixel;
                        positiveInfinity = definitions[i].UserSize.Value;
                        userMinSize = Math.Max(userMinSize, Math.Min(positiveInfinity, userMaxSize));
                        break;

                    case GridUnitType.Star:
                        if (!treatStarAsAuto)
                        {
                            definitions[i].SizeType = LayoutTimeSizeType.Star;
                            positiveInfinity = double.PositiveInfinity;
                            break;
                        }

                        definitions[i].SizeType = LayoutTimeSizeType.Auto;
                        positiveInfinity = double.PositiveInfinity;
                        break;
                }
                
                definitions[i].UpdateMinSize(userMinSize);
                definitions[i].MeasureSize = Math.Max(userMinSize, Math.Min(positiveInfinity, userMaxSize));
            }
        }

        private void ValidateCells()
        {
            if (this.CellsStructureDirty)
            {
                this.ValidateCellsCore();
                this.CellsStructureDirty = false;
            }
        }

        private void ValidateCellsCore()
        {
            UIElementCollection internalChildren = base.InternalChildren;
            ExtendedData extData = this.ExtData;
            extData.CellCachesCollection = new CellCache[internalChildren.Count];
            extData.CellGroup1 = int.MaxValue;
            extData.CellGroup2 = int.MaxValue;
            extData.CellGroup3 = int.MaxValue;
            extData.CellGroup4 = int.MaxValue;
            bool hasStarCellsU = false;
            bool hasStarCellsV = false;
            bool flag3 = false;
            for (int i = this.PrivateCells.Length - 1; i >= 0; i--)
            {
                UIElement element = internalChildren[i];
                if (element != null)
                {
                    CellCache cache = new CellCache();
                    cache.ColumnIndex = Math.Min(GetColumn(element), this.DefinitionsU.Length - 1);
                    cache.RowIndex = Math.Min(GetRow(element), this.DefinitionsV.Length - 1);
                    cache.ColumnSpan = Math.Min(GetColumnSpan(element), this.DefinitionsU.Length - cache.ColumnIndex);
                    cache.RowSpan = Math.Min(GetRowSpan(element), this.DefinitionsV.Length - cache.RowIndex);
                    cache.SizeTypeU = this.GetLengthTypeForRange(this.DefinitionsU, cache.ColumnIndex, cache.ColumnSpan);
                    cache.SizeTypeV = this.GetLengthTypeForRange(this.DefinitionsV, cache.RowIndex, cache.RowSpan);
                    hasStarCellsU |= cache.IsStarU;
                    hasStarCellsV |= cache.IsStarV;

                    if (!cache.IsStarV)
                    {
                        if (!cache.IsStarU)
                        {
                            cache.Next = extData.CellGroup1;
                            extData.CellGroup1 = i;
                        }
                        else
                        {
                            cache.Next = extData.CellGroup3;
                            extData.CellGroup3 = i;
                            flag3 |= cache.IsAutoV;
                        }
                    }
                    else if (cache.IsAutoU && !cache.IsStarU)
                    {
                        cache.Next = extData.CellGroup2;
                        extData.CellGroup2 = i;
                    }
                    else
                    {
                        cache.Next = extData.CellGroup4;
                        extData.CellGroup4 = i;
                    }

                    this.PrivateCells[i] = cache;
                }
            }
            this.HasStarCellsU = hasStarCellsU;
            this.HasStarCellsV = hasStarCellsV;
            this.HasGroup3CellsInAutoRows = flag3;
        }

        private LayoutTimeSizeType GetLengthTypeForRange(DefinitionBase[] definitions, int start, int count)
        {
            LayoutTimeSizeType none = LayoutTimeSizeType.None;
            int index = (start + count) - 1;
            do
            {
                none = (LayoutTimeSizeType)((byte)(none | definitions[index].SizeType));
            }
            while (--index >= start);
            return none;
        }

        private void MeasureCellsGroup(int cellsHead, Size referenceSize, bool ignoreDesiredSizeWidth, bool forceInfinityHeight)
        {
            if (cellsHead < this.PrivateCells.Length)
            {
                UIElementCollection internalChildren = base.InternalChildren;
                Hashtable store = null;
                bool infinityV = forceInfinityHeight;
                int cell = cellsHead;
                do
                {
                    this.MeasureCell(cell, forceInfinityHeight);
                    if (!ignoreDesiredSizeWidth)
                    {
                        if (this.PrivateCells[cell].ColumnSpan == 1)
                        {
                            this.DefinitionsU[this.PrivateCells[cell].ColumnIndex].UpdateMinSize(Math.Min(internalChildren[cell].DesiredSize.Width, this.DefinitionsU[this.PrivateCells[cell].ColumnIndex].UserMaxSize));
                        }
                        else
                        {
                            RegisterSpan(ref store, this.PrivateCells[cell].ColumnIndex, this.PrivateCells[cell].ColumnSpan, true, internalChildren[cell].DesiredSize.Width);
                        }
                    }

                    if (!infinityV)
                    {
                        if (this.PrivateCells[cell].RowSpan == 1)
                        {
                            this.DefinitionsV[this.PrivateCells[cell].RowIndex].UpdateMinSize(Math.Min(internalChildren[cell].DesiredSize.Height, this.DefinitionsV[this.PrivateCells[cell].RowIndex].UserMaxSize));
                        }
                        else
                        {
                            RegisterSpan(ref store, this.PrivateCells[cell].RowIndex, this.PrivateCells[cell].RowSpan, false, internalChildren[cell].DesiredSize.Height);
                        }
                    }

                    cell = this.PrivateCells[cell].Next;
                }
                while (cell < this.PrivateCells.Length);

                if (store != null)
                {
                    foreach (DictionaryEntry entry in store)
                    {
                        SpanKey key = (SpanKey)entry.Key;
                        double requestedSize = (double)entry.Value;
                        this.EnsureMinSizeInDefinitionRange(key.U ? this.DefinitionsU : this.DefinitionsV, key.Start, key.Count, requestedSize, key.U ? referenceSize.Width : referenceSize.Height);
                    }
                }
            }
        }

        private void MeasureCell(int cell, bool forceInfinityV)
        {
            double positiveInfinity;
            double num2;

            if (this.PrivateCells[cell].IsAutoU && !this.PrivateCells[cell].IsStarU)
            {
                positiveInfinity = double.PositiveInfinity;
            }
            else
            {
                positiveInfinity = this.GetMeasureSizeForRange(this.DefinitionsU, this.PrivateCells[cell].ColumnIndex, this.PrivateCells[cell].ColumnSpan);
            }

            if (forceInfinityV)
            {
                num2 = double.PositiveInfinity;
            }
            else if (this.PrivateCells[cell].IsAutoV && !this.PrivateCells[cell].IsStarV)
            {
                num2 = double.PositiveInfinity;
            }
            else
            {
                num2 = this.GetMeasureSizeForRange(this.DefinitionsV, this.PrivateCells[cell].RowIndex, this.PrivateCells[cell].RowSpan);
            }

            UIElement child = base.InternalChildren[cell];
            if (child != null)
            {
                Size childConstraint = new Size(positiveInfinity, num2);
                Helper.SetMeasureDataOnChild(this, child, childConstraint);
                child.Measure(childConstraint);
            }
        }

        private double GetMeasureSizeForRange(DefinitionBase[] definitions, int start, int count)
        {
            double num = 0.0;
            int index = (start + count) - 1;
            do
            {
                num += (definitions[index].SizeType == LayoutTimeSizeType.Auto) ? definitions[index].MinSize : definitions[index].MeasureSize;
            }
            while (--index >= start);
            return num;
        }

        private void ResolveStar(DefinitionBase[] definitions, double availableSize)
        {
            DefinitionBase[] tempDefinitions = this.TempDefinitions;
            int length = 0;
            double num2 = 0.0;

            for (int i = 0; i < definitions.Length; i++)
            {
                double num4;
                switch (definitions[i].SizeType)
                {
                    case LayoutTimeSizeType.Pixel:
                        {
                            num2 += definitions[i].MeasureSize;
                            continue;
                        }
                    case LayoutTimeSizeType.Auto:
                        {
                            num2 += definitions[i].MinSize;
                            continue;
                        }
                    case (LayoutTimeSizeType.Auto | LayoutTimeSizeType.Pixel):
                        {
                            continue;
                        }
                    case LayoutTimeSizeType.Star:
                        {
                            tempDefinitions[length++] = definitions[i];
                            num4 = definitions[i].UserSize.Value;
                            if (!_IsZero(num4))
                            {
                                break;
                            }
                            definitions[i].MeasureSize = 0.0;
                            definitions[i].SizeCache = 0.0;
                            continue;
                        }
                    default:
                        {
                            continue;
                        }
                }
                num4 = Math.Min(num4, 1E+298);
                definitions[i].MeasureSize = num4;
                double num5 = Math.Min(Math.Max(definitions[i].MinSize, definitions[i].UserMaxSize), 1E+298);
                definitions[i].SizeCache = num5 / num4;
            }

            if (length > 0)
            {
                Array.Sort(tempDefinitions, 0, length, s_starDistributionOrderComparer);
                double num6 = 0.0;
                int index = length - 1;
                do
                {
                    num6 += tempDefinitions[index].MeasureSize;
                    tempDefinitions[index].SizeCache = num6;
                }
                while (--index >= 0);

                index = 0;
                do
                {
                    double minSize;
                    double measureSize = tempDefinitions[index].MeasureSize;
                    if (_IsZero(measureSize))
                    {
                        minSize = tempDefinitions[index].MinSize;
                    }
                    else
                    {
                        double num10 = Math.Max((double)(availableSize - num2), (double)0.0) * (measureSize / tempDefinitions[index].SizeCache);
                        minSize = Math.Min(num10, tempDefinitions[index].UserMaxSize);
                        minSize = Math.Max(tempDefinitions[index].MinSize, minSize);
                    }

                    tempDefinitions[index].MeasureSize = minSize;
                    num2 += minSize;
                }
                while (++index < length);
            }
        }
    }

    private class ExtendedData
    {
        internal Grid.CellCache[] CellCachesCollection;
        internal int CellGroup1;
        internal int CellGroup2;
        internal int CellGroup3;
        internal int CellGroup4;
        internal ColumnDefinitionCollection ColumnDefinitions;
        internal DefinitionBase[] DefinitionsU;
        internal DefinitionBase[] DefinitionsV;
        internal RowDefinitionCollection RowDefinitions;
        internal DefinitionBase[] TempDefinitions;
    }

    private struct CellCache
    {
        internal int ColumnIndex;
        internal int RowIndex;
        internal int ColumnSpan;
        internal int RowSpan;
        internal Grid.LayoutTimeSizeType SizeTypeU;
        internal Grid.LayoutTimeSizeType SizeTypeV;
        internal int Next;

        internal bool IsStarU
        {
            get
            {
                return (((byte)(this.SizeTypeU & Grid.LayoutTimeSizeType.Star)) != 0);
            }
        }
        internal bool IsAutoU
        {
            get
            {
                return (((byte)(this.SizeTypeU & Grid.LayoutTimeSizeType.Auto)) != 0);
            }
        }
        internal bool IsStarV
        {
            get
            {
                return (((byte)(this.SizeTypeV & Grid.LayoutTimeSizeType.Star)) != 0);
            }
        }
        internal bool IsAutoV
        {
            get
            {
                return (((byte)(this.SizeTypeV & Grid.LayoutTimeSizeType.Auto)) != 0);
            }
        }
    }

    public enum GridUnitType
    {
        Auto,
        Pixel,
        Star
    }
}