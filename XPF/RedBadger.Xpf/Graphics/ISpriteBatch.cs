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

namespace RedBadger.Xpf.Graphics
{
    using RedBadger.Xpf.Media;

    /// <summary>
    ///     A batch of Sprites that should be sent to the GPU for rendering together.
    /// </summary>
    public interface ISpriteBatch
    {
        void Begin(Rect clippingRect);

        /// <summary>
        ///     Adds a <see cref = "ITexture">ITexture</see> sprite to the batch for rendering.
        /// </summary>
        /// <param name = "texture">The <see cref = "ITexture">ITexture</see> to render.</param>
        /// <param name = "rect">A <see cref = "Rect">Rect</see> defining the destination in pixels for drawing the <see cref = "ITexture">ITexture</see>.</param>
        /// <param name = "color">The <see cref = "Color">Color</see> to tint the <see cref = "ITexture">ITexture</see>.</param>
        void Draw(ITexture texture, Rect rect, Color color);

        /// <summary>
        ///     Adds a <see cref = "ISpriteFont">ISpriteFont</see> to the batch for rendering.
        /// </summary>
        /// <param name = "spriteFont">The <see cref = "ISpriteFont">ISpriteFont</see> to render.</param>
        /// <param name = "text">The text to render.</param>
        /// <param name = "position">A <see cref = "Point">Point</see> representing the position in pixels to render the text.</param>
        /// <param name = "color">The <see cref = "Color">Color</see> to tint the <see cref = "ISpriteFont">ISpriteFont</see>.</param>
        void DrawString(ISpriteFont spriteFont, string text, Point position, Color color);

        void End();

        /// <summary>
        ///     Tries to intersect the supplied clipping <see cref = "Rect">Rect</see> against the graphics device viewport to decide whether a draw is required.
        /// </summary>
        /// <remarks>This method will return true if clippingRect.IsEmpty is true because this indicates that no clipping is required.</remarks>
        /// <param name = "clippingRect">A Rect to intersect (<see cref="Rect.Intersect">Rect.Intersect</see>) with the Viewport.  Rect.Empty indicates no clipping.</param>
        /// <returns>True if all or part of the clippingRect is inside the viewport.</returns>
        bool TryIntersectViewport(ref Rect clippingRect);
    }
}
