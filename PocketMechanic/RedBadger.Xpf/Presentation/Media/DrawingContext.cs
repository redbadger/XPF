namespace RedBadger.Xpf.Presentation.Media
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using RedBadger.Xpf.Graphics;

    using Vector = RedBadger.Xpf.Presentation.Vector;

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
            foreach (ISpriteJob spriteJob in this.jobs)
            {
                spriteJob.Draw(spriteBatch);
            }
        }

        public void PreDraw()
        {
            Vector absoluteOffset = this.element.CalculateAbsoluteOffset();

            foreach (ISpriteJob spriteJob in this.jobs)
            {
                spriteJob.SetAbsoluteOffset(absoluteOffset);
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

        public void DrawText(ISpriteFont spriteFont, string text, Vector position, Brush brush)
        {
            this.jobs.Add(new SpriteFontJob(spriteFont, text, position, brush));
        }
    }
}