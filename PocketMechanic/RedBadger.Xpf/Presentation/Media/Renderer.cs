namespace RedBadger.Xpf.Presentation.Media
{
    using System.Collections.Generic;

    using RedBadger.Xpf.Graphics;

    public class Renderer : IRenderer
    {
        private readonly Dictionary<IElement, IDrawingContext> drawingContexts =
            new Dictionary<IElement, IDrawingContext>();

        private readonly IPrimitivesService primitivesService;

        private readonly ISpriteBatch spriteBatch;

        private bool isPreDrawRequired;

        public Renderer(ISpriteBatch spriteBatch, IPrimitivesService primitivesService)
        {
            this.spriteBatch = spriteBatch;
            this.primitivesService = primitivesService;
        }

        public void Clear()
        {
            this.drawingContexts.Clear();
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
            IDrawingContext drawingContext;

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
    }
}