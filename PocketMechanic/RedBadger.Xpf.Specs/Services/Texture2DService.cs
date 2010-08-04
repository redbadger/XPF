namespace RedBadger.Xpf.Specs.Services
{
    using System.IO;
    using System.Windows.Media.Imaging;

    using Microsoft.Xna.Framework.Graphics;

    public class Texture2DService
    {
        private Texture2D badger;

        public Texture2DService(IGraphicsDeviceService graphicsDeviceService)
        {
            this.CreateBadgerTexture(graphicsDeviceService);
        }

        public Texture2D Badger
        {
            get
            {
                return this.badger;
            }
        }

        private void CreateBadgerTexture(IGraphicsDeviceService graphicsDeviceService)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = File.OpenRead("badger.jpg");
            bitmapImage.EndInit();
            var bytes = new byte[bitmapImage.PixelWidth * bitmapImage.PixelHeight * 4];
            bitmapImage.CopyPixels(bytes, bitmapImage.PixelWidth * 4, 0);

            this.badger = new Texture2D(
                graphicsDeviceService.GraphicsDevice, bitmapImage.PixelWidth, bitmapImage.PixelHeight);
            this.Badger.SetData(bytes);
        }
    }
}