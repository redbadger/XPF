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

namespace RedBadger.Xpf.Internal
{
    internal struct MinMax
    {
        internal readonly double MaxHeight;

        internal readonly double MaxWidth;

        internal readonly double MinHeight;

        internal readonly double MinWidth;

        internal MinMax(UIElement element)
        {
            double height = element.Height;
            double minHeight = element.MinHeight;
            double maxHeight = element.MaxHeight;
            this.MaxHeight = (double.IsNaN(height) ? double.PositiveInfinity : height).Coerce(minHeight, maxHeight);
            this.MinHeight = (double.IsNaN(height) ? 0 : height).Coerce(minHeight, maxHeight);

            double width = element.Width;
            double minWidth = element.MinWidth;
            double maxWidth = element.MaxWidth;
            this.MaxWidth = (double.IsNaN(width) ? double.PositiveInfinity : width).Coerce(minWidth, maxWidth);
            this.MinWidth = (double.IsNaN(width) ? 0 : width).Coerce(minWidth, maxWidth);
        }
    }
}
