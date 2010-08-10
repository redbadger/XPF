namespace RedBadger.Xpf.Presentation.Media
{
    using System;
    using System.Collections.Generic;

    using RedBadger.Xpf.Graphics;

    internal class DrawingGroup
    {
        private readonly IList<SpriteJob> jobs = new List<SpriteJob>();

        public IList<SpriteJob> Jobs
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
    }
}