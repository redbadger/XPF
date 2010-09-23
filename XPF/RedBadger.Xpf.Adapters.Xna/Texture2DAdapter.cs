namespace RedBadger.Xpf.Adapters.Xna
{
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;

    /// <summary>
    ///     Adapts an XNA <see cref = "Texture2D">Texture2D</see> to an XPF <see cref = "ITexture2D">ITexture2D</see>.
    /// </summary>
    public class Texture2DAdapter : ITexture2D
    {
        private readonly Texture2D texture2D;

        /// <summary>
        ///     Initializes a new instance of a <see cref = "Texture2DAdapter">Texture2DAdapter</see>.
        /// </summary>
        /// <param name = "texture2D">The XNA <see cref = "Texture2D">Texture2D</see> to adapt.</param>
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

        /// <summary>
        ///     The adapted XNA <see cref = "Texture2D">Texture2D</see>.
        /// </summary>
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