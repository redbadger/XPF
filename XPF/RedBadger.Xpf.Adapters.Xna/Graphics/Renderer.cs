namespace RedBadger.Xpf.Adapters.Xna.Graphics
{
    using System.Collections.Generic;
    using System.Linq;

    using RedBadger.Xpf.Graphics;

    public class Renderer : IRenderer
    {
        private readonly Dictionary<IElement, DrawingContext> drawingContexts =
            new Dictionary<IElement, DrawingContext>();

        private readonly IPrimitivesService primitivesService;

        private readonly ISpriteBatch spriteBatch;

        private bool isPreDrawRequired;

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