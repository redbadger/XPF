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
    using System.Collections.Generic;
    using System.Linq;

    using RedBadger.Xpf.Graphics;

    public class Renderer : IRenderer
    {
        private readonly LinkedList<DrawingContext> drawList = new LinkedList<DrawingContext>();

        private readonly Dictionary<IElement, LinkedListNode<DrawingContext>> drawingContexts =
            new Dictionary<IElement, LinkedListNode<DrawingContext>>();

        private readonly IPrimitivesService primitivesService;

        private readonly ISpriteBatch spriteBatch;

        private bool isPreDrawRequired;

        private IElement rootElement;

        public Renderer(ISpriteBatch spriteBatch, IPrimitivesService primitivesService)
        {
            this.spriteBatch = spriteBatch;
            this.primitivesService = primitivesService;
        }

        public void ClearInvalidDrawingContexts()
        {
            this.ClearContextsWithOrphanedElements();

            foreach (var drawingContext in this.drawingContexts.Values)
            {
                drawingContext.Value.ClearIfInvalid();
            }
        }

        public void Draw()
        {
            if (this.drawList.Count != 0)
            {
                foreach (DrawingContext drawingContext in this.drawList)
                {
                    drawingContext.Draw(this.spriteBatch);
                }

                this.spriteBatch.End();
            }
        }

        public IDrawingContext GetDrawingContext(IElement element)
        {
            LinkedListNode<DrawingContext> node;

            if (this.drawingContexts.TryGetValue(element, out node))
            {
                node.Value.Clear();
            }
            else
            {
                node = new LinkedListNode<DrawingContext>(new DrawingContext(element, this.primitivesService));
                this.drawingContexts.Add(element, node);
            }

            this.isPreDrawRequired = true;
            return node.Value;
        }

        public void PreDraw()
        {
            if (this.isPreDrawRequired)
            {
                this.drawList.Clear();

                if (this.rootElement == null && this.drawingContexts.Count > 0)
                {
                    this.rootElement = this.drawingContexts.First().Key;
                }

                if (this.rootElement != null)
                {
                    this.PreDraw(this.rootElement, Vector.Zero, Rect.Empty);
                }

                this.isPreDrawRequired = false;
            }
        }

        private void ClearContextsWithOrphanedElements()
        {
            this.drawingContexts.Keys.Where(element => element.VisualParent == null).ToList().ForEach(
                orphan => this.drawingContexts.Remove(orphan));
        }

        private void PreDraw(IElement element, Vector absoluteOffset, Rect absoluteClippingRect)
        {
            LinkedListNode<DrawingContext> node;
            if (this.drawingContexts.TryGetValue(element, out node))
            {
                DrawingContext drawingContext = node.Value;

                absoluteOffset += element.VisualOffset;

                Rect clippingRect = element.ClippingRect;
                clippingRect.Displace(absoluteOffset);

                if (!absoluteClippingRect.IsEmpty)
                {
                    clippingRect.Intersect(absoluteClippingRect);
                    if (clippingRect.IsEmpty)
                    {
                        clippingRect = absoluteClippingRect;
                    }
                }

                drawingContext.AbsoluteOffset = absoluteOffset;
                drawingContext.AbsoluteClippingRect = clippingRect;
                this.drawList.AddLast(node);

                foreach (IElement child in element.GetVisualChildren())
                {
                    this.PreDraw(child, absoluteOffset, drawingContext.AbsoluteClippingRect);
                }
            }
        }
    }
}
