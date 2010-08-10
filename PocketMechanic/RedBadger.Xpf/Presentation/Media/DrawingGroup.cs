namespace RedBadger.Xpf.Presentation.Media
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;

    internal class DrawingGroup
    {
        private readonly IElement element;

        private readonly IList<ISpriteJob> jobs = new List<ISpriteJob>();

        public DrawingGroup(IElement element)
        {
            this.element = element;
        }

        public IList<ISpriteJob> Jobs
        {
            get
            {
                return this.jobs;
            }
        }

        public void Draw(ISpriteBatch spriteBatch)
        {
            foreach (var spriteJob in this.jobs)
            {
                spriteJob.Draw(spriteBatch);
            }
        }

        public void ResolveOffsets()
        {
            var absoluteOffset = this.element.AbsoluteOffset;
            if (absoluteOffset != Vector2.Zero)
            {
                foreach (var spriteJob in this.jobs)
                {
                    spriteJob.SetAbsoluteOffset(absoluteOffset);
                }
            }
        }
    }
}