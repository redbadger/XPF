namespace RedBadger.Xpf.Presentation.Media
{
    using System;

    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation.Media.Imaging;

    public struct SpriteImageJob : ISpriteJob
    {
        private readonly ImageSource imageSource;

        private readonly Rect rect;

        private Vector2 absoluteOffset;

        public SpriteImageJob(ImageSource imageSource, Rect rect)
        {
            this.imageSource = imageSource;
            this.rect = rect;
            this.absoluteOffset = Vector2.Zero;
        }

        public void Draw(ISpriteBatch spriteBatch)
        {
            var image = this.imageSource as XnaImage;
            if (image == null)
            {
                throw new NotImplementedException("Currently an ImageSource must be an XnaImage");
            }

            var drawRect = new Rect(this.rect.Position + this.absoluteOffset, this.rect.Size);
            spriteBatch.Draw(image.Texture, drawRect, Color.White);
        }

        public void SetAbsoluteOffset(Vector2 offset)
        {
            this.absoluteOffset = offset;
        }
    }
}