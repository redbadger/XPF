namespace RedBadger.Xpf.Specs.Services
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    internal class GraphicsDeviceService : IGraphicsDeviceService
    {
        public GraphicsDeviceService()
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

        public GraphicsDevice GraphicsDevice { get; private set; }
    }
}