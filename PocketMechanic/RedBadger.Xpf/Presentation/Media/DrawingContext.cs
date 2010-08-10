namespace RedBadger.Xpf.Presentation.Media
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;

    public class DrawingContext
    {
        private readonly IList<DrawingGroup> drawingGroups = new List<DrawingGroup>();

        private readonly IPrimitivesService primitivesService;

        private DrawingGroup currentDrawingGroup;

        public DrawingContext(IPrimitivesService primitivesService)
        {
            this.primitivesService = primitivesService;
        }

        public void Clear()
        {
            this.drawingGroups.Clear();
        }

        public void Close()
        {
            this.ThrowIfContextIsClosed();

            this.drawingGroups.Add(this.currentDrawingGroup);
            this.currentDrawingGroup = null;
        }

        public void Draw(ISpriteBatch spriteBatch)
        {
            this.ThrowIfContextIsOpen();

            foreach (var drawingGroup in this.drawingGroups)
            {
                drawingGroup.Draw(spriteBatch);
            }
        }

        public void DrawRectangle(Rect rect, Brush brush)
        {
            this.ThrowIfContextIsClosed();

            this.currentDrawingGroup.Jobs.Add(new SpriteTextureJob(this.primitivesService.SinglePixel, rect, brush));
        }

        public void DrawText(ISpriteFont spriteFont, string text, Brush brush)
        {
            this.DrawText(spriteFont, text, Vector2.Zero, brush);
        }

        public void DrawText(ISpriteFont spriteFont, string text, Vector2 position, Brush brush)
        {
            this.ThrowIfContextIsClosed();

            this.currentDrawingGroup.Jobs.Add(new SpriteFontJob(spriteFont, text, position, brush));
        }

        public void Open(IElement element)
        {
            this.ThrowIfContextIsOpen();

            this.currentDrawingGroup = new DrawingGroup(element);
        }

        public void ResolveOffsets()
        {
            foreach (var drawingGroup in this.drawingGroups)
            {
                drawingGroup.ResolveOffsets();
            }
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