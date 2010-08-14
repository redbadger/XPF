namespace RedBadger.Xpf.Presentation.Media
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation.Media.Imaging;

    using Vector = RedBadger.Xpf.Presentation.Vector;

    public struct SpriteImageJob : ISpriteJob
    {
        private readonly ImageSource imageSource;

        private readonly Rect rect;

        private Vector absoluteOffset;

        public SpriteImageJob(ImageSource imageSource, Rect rect)
        {
            this.imageSource = imageSource;
            this.rect = rect;
            this.absoluteOffset = Vector.Zero;
        }

        public void Draw(ISpriteBatch spriteBatch)
        {
            var image = this.imageSource as XnaImage;
            if (image == null)
            {
                throw new NotImplementedException("Currently an ImageSource must be an XnaImage");
            }

            var drawRect = this.rect != Rect.Empty ? this.rect : new Rect();
            drawRect.X += this.absoluteOffset.X;
            drawRect.Y += this.absoluteOffset.Y;

            spriteBatch.Draw(image.Texture, drawRect, Colors.White);
        }

        public void SetAbsoluteOffset(Vector offset)
        {
            this.absoluteOffset = offset;
        }
    }
}