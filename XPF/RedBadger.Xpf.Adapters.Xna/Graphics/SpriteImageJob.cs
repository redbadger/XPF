namespace RedBadger.Xpf.Adapters.Xna.Graphics
{
    using System;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Media;
    using RedBadger.Xpf.Media.Imaging;

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
            var image = this.imageSource as TextureImage;
            if (image == null)
            {
                throw new NotImplementedException("Currently an ImageSource must be an TextureImage");
            }

            Rect drawRect = !this.rect.IsEmpty ? this.rect : new Rect();
            drawRect.Displace(offset);

            spriteBatch.Draw(image.Texture, drawRect, Colors.White);
        }
    }
}