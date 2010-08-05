namespace RedBadger.Xpf.Specs
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class GraphicsDeviceService : IGraphicsDeviceService
    {
        private static GraphicsDeviceService singletonInstance;

        private GraphicsDeviceService()
        {
            GraphicsAdapter.UseNullDevice = true;
            GraphicsAdapter.UseReferenceDevice = true;
            this.GraphicsDevice = new GraphicsDevice(
                GraphicsAdapter.DefaultAdapter, 
                GraphicsProfile.Reach, 
                new PresentationParameters { DeviceWindowHandle = new Game().Window.Handle });
        }

        public event EventHandler<EventArgs> DeviceCreated;

        public event EventHandler<EventArgs> DeviceDisposing;

        public event EventHandler<EventArgs> DeviceReset;

        public event EventHandler<EventArgs> DeviceResetting;

        public static GraphicsDeviceService Instance
        {
            get
            {
                return singletonInstance ?? (singletonInstance = new GraphicsDeviceService());
            }
        }

        public GraphicsDevice GraphicsDevice { get; private set; }
    }
}