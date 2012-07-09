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

namespace RedBadger.Xpf.Adapters.Xna.Graphics
{
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;

    /// <summary>
    ///     Adapts an XNA <see cref = "Texture2D">Texture2D</see> to an XPF <see cref = "ITexture">ITexture</see>.
    /// </summary>
    public class Texture2DAdapter : ITexture
    {
        private readonly Texture2D texture2D;

        /// <summary>
        ///     Initializes a new instance of a <see cref = "Texture2DAdapter">Texture2DAdapter</see>.
        /// </summary>
        /// <param name = "texture2D">The XNA <see cref = "Texture2D">Texture2D</see> to adapt.</param>
        public Texture2DAdapter(Texture2D texture2D)
        {
            this.texture2D = texture2D;
        }

        public int Height
        {
            get
            {
                return this.texture2D.Height;
            }
        }

        /// <summary>
        ///     The adapted XNA <see cref = "Texture2D">Texture2D</see>.
        /// </summary>
        public Texture2D Value
        {
            get
            {
                return this.texture2D;
            }
        }

        public int Width
        {
            get
            {
                return this.texture2D.Width;
            }
        }
    }
}
