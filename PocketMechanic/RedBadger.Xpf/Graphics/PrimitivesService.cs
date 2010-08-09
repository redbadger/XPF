namespace RedBadger.Xpf.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class PrimitivesService : IPrimitivesService
    {
        private readonly GraphicsDevice graphicsDevice;

        public PrimitivesService(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this.SinglePixel = new Texture2DAdapter(CreateSinglePixel(this.graphicsDevice, 1, 1, Color.White));
        }

        public ITexture2D SinglePixel { get; private set; }

        private static Texture2D CreateSinglePixel(GraphicsDevice graphicsDevice, int width, int height, Color color)
        {
            // create the rectangle texture without colors
            var texture = new Texture2D(
                graphicsDevice,
                width,
                height,
                false,
                SurfaceFormat.Color);

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