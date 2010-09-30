namespace RedBadger.Xpf.Adapters.Xna.Graphics
{
    using System;
    using System.Collections.Generic;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Media;

    public class DrawingContext : IDrawingContext
    {
        private readonly IElement element;

        private readonly IList<ISpriteJob> jobs = new List<ISpriteJob>();

        private readonly IPrimitivesService primitivesService;

        private Rect absoluteClippingRect = Rect.Empty;

        private Vector absoluteOffset;

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

        public Rect AbsoluteClippingRect
        {
            get
            {
                return this.absoluteClippingRect;
            }

            set
            {
                this.absoluteClippingRect = value;
            }
        }

        public Vector AbsoluteOffset
        {
            get
            {
                return this.absoluteOffset;
            }

            set
            {
                this.absoluteOffset = value;
            }
        }

        public IElement Element
        {
            get
            {
                return this.element;
            }
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
            Rect clippingRect = this.absoluteClippingRect;
            if (spriteBatch.TryIntersectViewport(ref clippingRect))
            {
                spriteBatch.Begin(clippingRect);

                foreach (ISpriteJob spriteJob in this.jobs)
                {
                    spriteJob.Draw(spriteBatch, this.absoluteOffset);
                }
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

        public void DrawText(ISpriteFont spriteFont, string text, Point position, Brush brush)
        {
            this.jobs.Add(new SpriteFontJob(spriteFont, text, position, brush));
        }
    }
}