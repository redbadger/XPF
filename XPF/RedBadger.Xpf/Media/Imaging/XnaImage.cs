namespace RedBadger.Xpf.Media.Imaging
{
    using System;

    using RedBadger.Xpf.Graphics;

    public class XnaImage : BitmapImage
    {
        private readonly ITexture2D texture;

        public XnaImage(ITexture2D texture)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }

            this.texture = texture;
            this.PixelHeight = this.Texture.Height;
            this.PixelWidth = this.Texture.Width;
        }

        public ITexture2D Texture
        {
            get
            {
                return this.texture;
            }
        }
    }
}