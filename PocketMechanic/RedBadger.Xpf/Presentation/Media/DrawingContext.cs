namespace RedBadger.Xpf.Presentation.Media
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;

    public class DrawingContext : IDrawingContext
    {
        private readonly IElement element;

        private readonly IList<ISpriteJob> jobs = new List<ISpriteJob>();

        private readonly IPrimitivesService primitivesService;

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

        public void Draw(ISpriteBatch spriteBatch)
        {
            foreach (ISpriteJob spriteJob in this.jobs)
            {
                spriteJob.Draw(spriteBatch);
            }
        }

        public void DrawRectangle(Rect rect, Brush brush)
        {
            this.jobs.Add(new SpriteTextureJob(this.primitivesService.SinglePixel, rect, brush));
        }

        public void DrawText(ISpriteFont spriteFont, string text, Brush brush)
        {
            this.DrawText(spriteFont, text, Vector2.Zero, brush);
        }

        public void DrawText(ISpriteFont spriteFont, string text, Vector2 position, Brush brush)
        {
            this.jobs.Add(new SpriteFontJob(spriteFont, text, position, brush));
        }

        public void PreDraw()
        {
            Vector2 absoluteOffset = this.element.AbsoluteOffset;
            if (absoluteOffset != Vector2.Zero)
            {
                foreach (ISpriteJob spriteJob in this.jobs)
                {
                    spriteJob.SetAbsoluteOffset(absoluteOffset);
                }
            }
        }
    }
}