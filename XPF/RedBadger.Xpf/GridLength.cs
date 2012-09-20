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

namespace RedBadger.Xpf
{
    using System;

    public struct GridLength
    {
        private readonly GridUnitType gridUnitType;

        private readonly double value;

        static GridLength()
        {
            Auto = new GridLength(1, GridUnitType.Auto);
        }

        public GridLength(double value)
            : this(value, GridUnitType.Pixel)
        {
        }

        public GridLength(double value, GridUnitType gridUnitType)
        {
            if (double.IsNaN(value))
            {
                throw new ArgumentException();
            }

            if (double.IsInfinity(value))
            {
                throw new ArgumentException();
            }

            this.value = gridUnitType == GridUnitType.Auto ? 1 : value;
            this.gridUnitType = gridUnitType;
        }

        /// <summary>
        ///     Gets a <see cref = "GridLength">GridLength</see> whose <see cref = "GridUnitType">GridUnitType</see> is set to <see cref = "RedBadger.Xpf.GridUnitType.Auto">GridUnitType.Auto</see>.
        /// </summary>
        public static GridLength Auto
        {
            get;
            private set;
        }

        public GridUnitType GridUnitType
        {
            get
            {
                return this.gridUnitType;
            }
        }

        public double Value
        {
            get
            {
                return this.value;
            }
        }
    }
}
