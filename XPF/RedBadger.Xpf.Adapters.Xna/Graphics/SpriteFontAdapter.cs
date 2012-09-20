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
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;

    /// <summary>
    ///     Adapts an XNA <see cref = "SpriteFont">SpriteFont</see> to an XPF <see cref = "ISpriteFont">ISpriteFont</see>.
    /// </summary>
    public class SpriteFontAdapter : ISpriteFont
    {
        private readonly SpriteFont spriteFont;

        /// <summary>
        ///     Initializes a new instance of a <see cref = "SpriteFontAdapter">SpriteFontAdapter</see>.
        /// </summary>
        /// <param name = "spriteFont">The XNA <see cref = "SpriteFont">SpriteFont</see> to adapt.</param>
        public SpriteFontAdapter(SpriteFont spriteFont)
        {
            this.spriteFont = spriteFont;
        }

        /// <summary>
        ///     The adapted XNA <see cref = "SpriteFont">SpriteFont</see>.
        /// </summary>
        public SpriteFont Value
        {
            get
            {
                return this.spriteFont;
            }
        }

        public Size MeasureString(string text)
        {
            Vector2 size = this.spriteFont.MeasureString(text ?? string.Empty);
            return new Size(size.X, size.Y);
        }
    }
}
