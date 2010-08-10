namespace RedBadger.Xpf.Presentation.Media
{
    using System.Collections.Generic;

    using RedBadger.Xpf.Graphics;

    internal class DrawingGroup
    {
        private readonly IList<ISpriteJob> jobs = new List<ISpriteJob>();

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
    }
}