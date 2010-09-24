namespace RedBadger.Xpf.Adapters.Xna.Graphics
{
    using System.Collections.Generic;
    using System.Linq;

    using RedBadger.Xpf.Extensions;
    using RedBadger.Xpf.Graphics;

    public class Renderer : IRenderer
    {
        private readonly Stack<IDrawingContext> clipRegions = new Stack<IDrawingContext>();

        private readonly Dictionary<IElement, DrawingContext> drawingContexts =
            new Dictionary<IElement, DrawingContext>();

        private readonly IPrimitivesService primitivesService;

        private readonly ISpriteBatch spriteBatch;

        private bool isPreDrawRequired;

        private IDrawingContext parentContext;

        public Renderer(ISpriteBatch spriteBatch, IPrimitivesService primitivesService)
        {
            this.spriteBatch = spriteBatch;
            this.primitivesService = primitivesService;
        }

        public void ClearInvalidDrawingContexts()
        {
            this.ClearContextsWithOrphanedElements();

            foreach (DrawingContext drawingContext in this.drawingContexts.Values)
            {
                drawingContext.ClearIfInvalid();
            }
        }

        public void Draw()
        {
            foreach (DrawingContext drawingContext in this.drawingContexts.Values)
            {
                drawingContext.Draw(this.spriteBatch);
            }

            this.spriteBatch.End();
        }

        public IDrawingContext GetDrawingContext(IElement element)
        {
            DrawingContext drawingContext;

            if (this.drawingContexts.TryGetValue(element, out drawingContext))
            {
                drawingContext.Clear();
            }
            else
            {
                drawingContext = new DrawingContext(element, this.primitivesService);
                this.drawingContexts.Add(element, drawingContext);
            }

            this.isPreDrawRequired = true;
            return drawingContext;
        }

        public void PreDraw()
        {
            if (this.isPreDrawRequired)
            {
                foreach (DrawingContext drawingContext in this.drawingContexts.Values)
                {
                    drawingContext.PreDraw();

                    while (this.clipRegions.Count > 0 &&
                           !drawingContext.Element.IsDescendantOf(this.clipRegions.Peek().Element))
                    {
                        this.clipRegions.Pop();
                        this.parentContext = this.clipRegions.Count == 0 ? null : this.clipRegions.Peek();
                    }

                    if (!drawingContext.ClippingRect.IsEmpty)
                    {
                        this.parentContext = this.clipRegions.Count == 0 ? null : this.clipRegions.Peek();

                        var absoluteClippingRect = drawingContext.ClippingRect;
                        absoluteClippingRect.Displace(drawingContext.AbsoluteOffset);

                        if (this.parentContext != null)
                        {
                            absoluteClippingRect.Intersect(this.parentContext.AbsoluteClippingRect);
                            if (absoluteClippingRect.IsEmpty)
                            {
                                absoluteClippingRect = this.parentContext.AbsoluteClippingRect;
                            }
                        }

                        drawingContext.AbsoluteClippingRect = absoluteClippingRect;
                        this.clipRegions.Push(drawingContext);
                    }
                    else
                    {
                        drawingContext.AbsoluteClippingRect = this.clipRegions.Count > 0
                                                                  ? this.clipRegions.Peek().AbsoluteClippingRect
                                                                  : Rect.Empty;
                    }
                }

                this.isPreDrawRequired = false;
            }
        }

        private void ClearContextsWithOrphanedElements()
        {
            this.drawingContexts.Keys.Where(element => element.VisualParent == null).ToList().ForEach(
                orphan => this.drawingContexts.Remove(orphan));
        }
    }
}