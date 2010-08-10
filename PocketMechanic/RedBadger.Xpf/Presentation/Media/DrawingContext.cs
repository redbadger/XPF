namespace RedBadger.Xpf.Presentation.Media
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;

    public class DrawingContext
    {
        private readonly IPrimitivesService primitivesService;

        private readonly IList<DrawingGroup> drawingGroups = new List<DrawingGroup>();

        private DrawingGroup currentDrawingGroup;

        public DrawingContext(IPrimitivesService primitivesService)
        {
            this.primitivesService = primitivesService;
        }

        public void Close()
        {
            this.ThrowIfContextIsClosed();

            this.drawingGroups.Add(this.currentDrawingGroup);
            this.currentDrawingGroup = null;
        }

        public void DrawRectangle(Rect rect, Brush brush)
        {
            this.ThrowIfContextIsClosed();

            this.currentDrawingGroup.Jobs.Add(new SpriteTextureJob(this.primitivesService.SinglePixel, rect, brush));
        }

        public void DrawText(ISpriteFont spriteFont, string text, Color color)
        {
            this.ThrowIfContextIsClosed();

            this.currentDrawingGroup.Jobs.Add(new SpriteFontJob(spriteFont, text, color));
        }

        public void Flush(ISpriteBatch spriteBatch)
        {
            this.ThrowIfContextIsOpen();

            foreach (var drawingGroup in this.drawingGroups)
            {
                drawingGroup.Draw(spriteBatch);
            }
        }

        public void Open()
        {
            this.ThrowIfContextIsOpen();

            this.currentDrawingGroup = new DrawingGroup();
        }

        private void ThrowIfContextIsClosed()
        {
            if (this.currentDrawingGroup == null)
            {
                throw new InvalidOperationException(
                    "The operation you're trying to perform is invalid whilst the DrawingContext is closed");
            }
        }

        private void ThrowIfContextIsOpen()
        {
            if (this.currentDrawingGroup != null)
            {
                throw new InvalidOperationException(
                    "The operation you're trying to perform is invalid whilst the DrawingContext is open");
            }
        }
    }
}