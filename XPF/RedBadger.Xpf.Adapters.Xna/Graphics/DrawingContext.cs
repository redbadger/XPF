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
    using System;
    using System.Collections.Generic;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Media;

    public class DrawingContext : IDrawingContext
    {
        private readonly IElement element;

        private readonly IList<ISpriteJob> jobs = new List<ISpriteJob>();

        private readonly IPrimitivesService primitivesService;

        private Rect absoluteClippingRect = Rect.Empty;

        private Vector absoluteOffset;

        public DrawingContext(IElement element, IPrimitivesService primitivesService)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (primitivesService == null)
            {
                throw new ArgumentNullException("primitivesService");
            }

            this.element = element;
            this.primitivesService = primitivesService;
        }

        public Rect AbsoluteClippingRect
        {
            get
            {
                return this.absoluteClippingRect;
            }

            set
            {
                this.absoluteClippingRect = value;
            }
        }

        public Vector AbsoluteOffset
        {
            get
            {
                return this.absoluteOffset;
            }

            set
            {
                this.absoluteOffset = value;
            }
        }

        public IElement Element
        {
            get
            {
                return this.element;
            }
        }

        public void Clear()
        {
            this.jobs.Clear();
        }

        public void ClearIfInvalid()
        {
            if (!this.element.IsArrangeValid)
            {
                this.Clear();
            }
        }

        public void Draw(ISpriteBatch spriteBatch)
        {
            Rect clippingRect = this.absoluteClippingRect;
            if (spriteBatch.TryIntersectViewport(ref clippingRect))
            {
                spriteBatch.Begin(clippingRect);

                foreach (ISpriteJob spriteJob in this.jobs)
                {
                    spriteJob.Draw(spriteBatch, this.absoluteOffset);
                }
            }
        }

        public void DrawImage(ImageSource imageSource, Rect rect)
        {
            this.jobs.Add(new SpriteImageJob(imageSource, rect));
        }

        public void DrawRectangle(Rect rect, Brush brush)
        {
            this.jobs.Add(new SpriteTextureJob(this.primitivesService.SinglePixel, rect, brush));
        }

        public void DrawText(ISpriteFont spriteFont, string text, Point position, Brush brush)
        {
            this.jobs.Add(new SpriteFontJob(spriteFont, text, position, brush));
        }
    }
}
