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

        private Vector absoluteOffset;

        private Rect clippingRect = Rect.Empty;

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

        public Vector AbsoluteOffset
        {
            get
            {
                return this.absoluteOffset;
            }
        }

        public Rect ClippingRect
        {
            get
            {
                return this.clippingRect;
            }

            set
            {
                this.clippingRect = value;
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
            spriteBatch.Begin(this);

            foreach (ISpriteJob spriteJob in this.jobs)
            {
                spriteJob.Draw(spriteBatch, this.absoluteOffset);
            }
        }

        public void PreDraw()
        {
            this.absoluteOffset = this.element.CalculateAbsoluteOffset();
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