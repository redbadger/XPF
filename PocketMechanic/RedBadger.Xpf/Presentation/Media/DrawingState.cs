namespace RedBadger.Xpf.Presentation.Media
{
    using System.Collections.Generic;

    using RedBadger.Xpf.Graphics;

    public class DrawingState
    {
        private readonly IList<DrawingContext> drawingContexts = new List<DrawingContext>();

        private readonly IPrimitivesService primitivesService;

        public DrawingState(IPrimitivesService primitivesService)
        {
            this.primitivesService = primitivesService;
        }

        public void Clear()
        {
            this.drawingContexts.Clear();
        }

        public void Draw(ISpriteBatch spriteBatch)
        {
            foreach (DrawingContext drawingContext in this.drawingContexts)
            {
                drawingContext.Draw(spriteBatch);
            }
        }

        public DrawingContext GetDrawingContext(IElement element)
        {
            var drawingContext = new DrawingContext(element, this.primitivesService);
            this.drawingContexts.Add(drawingContext);
            return drawingContext;
        }

        public void ResolveOffsets()
        {
            foreach (DrawingContext drawingContext in this.drawingContexts)
            {
                drawingContext.ResolveOffsets();
            }
        }
    }
}