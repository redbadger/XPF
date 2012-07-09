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

namespace RedBadger.Xpf.Input
{
    using System.Diagnostics;

    [DebuggerDisplay("{Type} @ P:{Point} D:{Delta}")]
    public struct Gesture
    {
        public Vector Delta;

        public Point Point;

        public GestureType Type;

        public Gesture(GestureType type, Point point)
            : this(type, point, Vector.Zero)
        {
        }

        public Gesture(GestureType type, Point point, Vector delta)
        {
            this.Type = type;
            this.Point = point;
            this.Delta = delta;
        }

        public override string ToString()
        {
            return string.Format("Type: {0}, Point: {1}, Delta: {2}", this.Type, this.Point, this.Delta);
        }
    }
}
