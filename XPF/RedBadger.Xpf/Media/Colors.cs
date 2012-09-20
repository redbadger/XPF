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

namespace RedBadger.Xpf.Media
{
    public static class Colors
    {
        public static Color Black
        {
            get
            {
                return Color.FromUInt32(0xff000000);
            }
        }

        public static Color Blue
        {
            get
            {
                return Color.FromUInt32(0xff0000ff);
            }
        }

        public static Color Brown
        {
            get
            {
                return Color.FromUInt32(0xffa52a2a);
            }
        }

        public static Color Cyan
        {
            get
            {
                return Color.FromUInt32(0xff00ffff);
            }
        }

        public static Color DarkGray
        {
            get
            {
                return Color.FromUInt32(0xffa9a9a9);
            }
        }

        public static Color Gray
        {
            get
            {
                return Color.FromUInt32(0xff808080);
            }
        }

        public static Color Green
        {
            get
            {
                return Color.FromUInt32(0xff008000);
            }
        }

        public static Color LightGray
        {
            get
            {
                return Color.FromUInt32(0xffd3d3d3);
            }
        }

        public static Color Magenta
        {
            get
            {
                return Color.FromUInt32(0xffff00ff);
            }
        }

        public static Color Orange
        {
            get
            {
                return Color.FromUInt32(0xffffa500);
            }
        }

        public static Color Purple
        {
            get
            {
                return Color.FromUInt32(0xff800080);
            }
        }

        public static Color Red
        {
            get
            {
                return Color.FromUInt32(0xffff0000);
            }
        }

        public static Color Transparent
        {
            get
            {
                return Color.FromUInt32(0xffffff);
            }
        }

        public static Color White
        {
            get
            {
                return Color.FromUInt32(uint.MaxValue);
            }
        }

        public static Color Yellow
        {
            get
            {
                return Color.FromUInt32(0xffffff00);
            }
        }
    }
}
