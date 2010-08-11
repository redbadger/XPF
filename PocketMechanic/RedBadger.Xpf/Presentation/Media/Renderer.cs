namespace RedBadger.Xpf.Presentation.Media
{
    using System.Collections.Generic;

    using RedBadger.Xpf.Graphics;

    public class Renderer : IRenderer
    {
        private readonly IList<DrawingContext> drawingContexts = new List<DrawingContext>();

        private readonly IPrimitivesService primitivesService;

        private readonly ISpriteBatch spriteBatch;

        public Renderer(ISpriteBatch spriteBatch, IPrimitivesService primitivesService)
        {
            this.spriteBatch = spriteBatch;
            this.primitivesService = primitivesService;
        }

        public void Draw()
        {
            foreach (DrawingContext drawingContext in this.drawingContexts)
            {
                drawingContext.Draw(this.spriteBatch);
            }
        }

        public IDrawingContext GetDrawingContext(IElement element)
        {
            var drawingContext = new DrawingContext(element, this.primitivesService);
            this.drawingContexts.Add(drawingContext);
            return drawingContext;
        }

        public void PreDraw()
        {
            foreach (DrawingContext drawingContext in this.drawingContexts)
            {
                drawingContext.PreDraw();
            }
        }
    }
}