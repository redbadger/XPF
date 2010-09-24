namespace RedBadger.Xpf.Media.Imaging
{
    using System;

    using RedBadger.Xpf.Graphics;

    public class TextureImage : BitmapSource
    {
        private readonly ITexture texture;

        public TextureImage(ITexture texture)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }

            this.texture = texture;
            this.PixelHeight = this.Texture.Height;
            this.PixelWidth = this.Texture.Width;
        }

        public ITexture Texture
        {
            get
            {
                return this.texture;
            }
        }
    }
}