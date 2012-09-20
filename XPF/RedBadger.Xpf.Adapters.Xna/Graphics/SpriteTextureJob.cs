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
    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Media;

    public struct SpriteTextureJob : ISpriteJob
    {
        private readonly Brush brush;

        private readonly Rect rect;

        private readonly ITexture texture;

        public SpriteTextureJob(ITexture texture, Rect rect, Brush brush)
        {
            this.texture = texture;
            this.rect = rect;
            this.brush = brush;
        }

        public void Draw(ISpriteBatch spriteBatch, Vector offset)
        {
            Rect drawRect = !this.rect.IsEmpty ? this.rect : new Rect();
            drawRect.Displace(offset);

            var solidColorBrush = this.brush as SolidColorBrush;
            spriteBatch.Draw(this.texture, drawRect, solidColorBrush != null ? solidColorBrush.Color : Colors.Magenta);
        }
    }
}
