namespace RedBadger.Xpf.Adapters.Xna.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;

    /// <summary>
    ///     Provides primitives that XPF requires to render correctly.
    /// </summary>
    public class PrimitivesService : IPrimitivesService
    {
        private readonly GraphicsDevice graphicsDevice;

        /// <summary>
        ///     Initializes a new instance of the <see cref = "PrimitivesService">PrimitivesService</see>.
        /// </summary>
        /// <param name = "graphicsDevice">An XNA <see cref = "GraphicsDevice">GraphicsDevice</see> that can be used to generate primitives.</param>
        public PrimitivesService(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this.SinglePixel = new Texture2DAdapter(CreateSinglePixel(this.graphicsDevice, 1, 1, Color.White));
        }

        public ITexture2D SinglePixel { get; private set; }

        private static Texture2D CreateSinglePixel(GraphicsDevice graphicsDevice, int width, int height, Color color)
        {
            // create the rectangle texture without colors
            var texture = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);

            // Create a color array for the pixels
            var colors = new Color[width * height];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(color.ToVector3());
            }

            // Set the color data for the texture
            texture.SetData(colors);

            return texture;
        }
    }
}