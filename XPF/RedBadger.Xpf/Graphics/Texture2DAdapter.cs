namespace RedBadger.Xpf.Graphics
{
    using Microsoft.Xna.Framework.Graphics;

    public class Texture2DAdapter : ITexture2D
    {
        private readonly Texture2D texture2D;

        public Texture2DAdapter(Texture2D texture2D)
        {
            this.texture2D = texture2D;
        }

        public int Height
        {
            get
            {
                return this.texture2D.Height;
            }
        }

        public Texture2D Value
        {
            get
            {
                return this.texture2D;
            }
        }

        public int Width
        {
            get
            {
                return this.texture2D.Width;
            }
        }
    }
}