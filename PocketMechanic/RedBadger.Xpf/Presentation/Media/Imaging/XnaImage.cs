namespace RedBadger.Xpf.Presentation.Media.Imaging
{
    using System;

    using Microsoft.Xna.Framework.Graphics;

    public class XnaImage : BitmapImage
    {
        private readonly Texture2D texture;

        public XnaImage(Texture2D texture)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }

            this.texture = texture;
            this.PixelHeight = this.texture.Height;
            this.PixelWidth = this.texture.Width;
        }
    }
}