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

    using Color = RedBadger.Xpf.Media.Color;
    using Point = RedBadger.Xpf.Point;

    /// <summary>
    ///     Adapts an XNA <see cref = "SpriteBatch">SpriteBatch</see> to an XPF <see cref = "ISpriteBatch">ISpriteBatch</see>.
    /// </summary>
    public class SpriteBatchAdapter : ISpriteBatch
    {
        private static readonly RasterizerState ScissorTestingRasterizerState = new RasterizerState
            {
               ScissorTestEnable = true, CullMode = CullMode.None 
            };

        private readonly SpriteBatch spriteBatch;

        private Rect? currentClippingRect;

        private bool isBatchOpen;

        /// <summary>
        ///     Initializes a new instance of a <see cref = "SpriteBatchAdapter">SpriteBatchAdapter</see>.
        /// </summary>
        /// <param name = "spriteBatch">The XNA <see cref = "SpriteBatch">SpriteBatch</see> to adapt.</param>
        public SpriteBatchAdapter(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        public void Begin(Rect clippingRect)
        {
            if (this.currentClippingRect != clippingRect)
            {
                this.currentClippingRect = clippingRect;

                if (this.isBatchOpen)
                {
                    this.End();
                }

                if (clippingRect.IsEmpty)
                {
                    this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                }
                else
                {
                    this.spriteBatch.GraphicsDevice.ScissorRectangle = clippingRect.ToRectangle();

                    this.spriteBatch.Begin(
                        SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, ScissorTestingRasterizerState);
                }

                this.isBatchOpen = true;
            }
        }

        public void Draw(ITexture texture, Rect rect, Color color)
        {
            var texture2DAdapter = texture as Texture2DAdapter;
            if (texture2DAdapter != null && texture2DAdapter.Value != null)
            {
                var rectangle = new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
                this.spriteBatch.Draw(
                    texture2DAdapter.Value, 
                    rectangle, 
                    new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A));
            }
        }

        public void DrawString(ISpriteFont spriteFont, string text, Point position, Color color)
        {
            var spriteFontAdapter = spriteFont as SpriteFontAdapter;
            if (spriteFontAdapter != null && spriteFontAdapter.Value != null)
            {
                this.spriteBatch.DrawString(
                    spriteFontAdapter.Value, 
                    text ?? string.Empty, 
                    new Vector2((int)position.X, (int)position.Y),
                    new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A));
            }
        }

        public void End()
        {
            if (this.isBatchOpen)
            {
                this.spriteBatch.End();
                this.currentClippingRect = null;

                this.isBatchOpen = false;
            }
        }

        public bool TryIntersectViewport(ref Rect clippingRect)
        {
            if (clippingRect.IsEmpty)
            {
                return true;
            }

            clippingRect.Intersect(this.spriteBatch.GraphicsDevice.Viewport.ToRect());
            return !clippingRect.IsEmpty;
        }
    }
}
