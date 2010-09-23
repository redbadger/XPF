namespace RedBadger.Xpf.Adapters.Xna
{
    using System;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Media;
    using RedBadger.Xpf.Presentation.Media.Imaging;

    public struct SpriteImageJob : ISpriteJob
    {
        private readonly ImageSource imageSource;

        private readonly Rect rect;

        public SpriteImageJob(ImageSource imageSource, Rect rect)
        {
            this.imageSource = imageSource;
            this.rect = rect;
        }

        public void Draw(ISpriteBatch spriteBatch, Vector offset)
        {
            var image = this.imageSource as XnaImage;
            if (image == null)
            {
                throw new NotImplementedException("Currently an ImageSource must be an XnaImage");
            }

            Rect drawRect = this.rect != Rect.Empty ? this.rect : new Rect();
            drawRect.X += offset.X;
            drawRect.Y += offset.Y;

            spriteBatch.Draw(image.Texture, drawRect, Colors.White);
        }
    }
}