#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

namespace RedBadger.Xpf.Controls
{
    using System;

    public abstract class DefinitionBase : ReactiveObject
    {
        private readonly DefinitionType definitionType;

        protected DefinitionBase(DefinitionType definitionType)
        {
            this.definitionType = definitionType;
        }

        protected enum DefinitionType
        {
            Column, 
            Row
        }

        internal double AvailableLength { get; set; }

        internal double Denominator { get; set; }

        internal double FinalLength { get; set; }

        internal double FinalOffset { get; set; }

        internal GridUnitType LengthType { get; set; }

        internal double MinLength { get; set; }

        internal double Numerator { get; set; }

        internal double StarAllocationOrder { get; set; }

        internal GridLength UserLength
        {
            get
            {
                return this.definitionType == DefinitionType.Column
                           ? this.GetValue(ColumnDefinition.WidthProperty)
                           : this.GetValue(RowDefinition.HeightProperty);
            }
        }

        internal double UserMaxLength
        {
            get
            {
                return this.definitionType == DefinitionType.Column
                           ? this.GetValue(ColumnDefinition.MaxWidthProperty)
                           : this.GetValue(RowDefinition.MaxHeightProperty);
            }
        }

        internal double UserMinLength
        {
            get
            {
                return this.definitionType == DefinitionType.Column
                           ? this.GetValue(ColumnDefinition.MinWidthProperty)
                           : this.GetValue(RowDefinition.MinHeightProperty);
            }
        }

        internal void UpdateMinLength(double minLength)
        {
            this.MinLength = Math.Max(this.MinLength, minLength);
        }
    }
}
