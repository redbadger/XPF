namespace RedBadger.Xpf.Specs.Services
{
    using Microsoft.Xna.Framework.Graphics;

    using StructureMap;

    public static class IOC
    {
        private static readonly object syncRoot = new object();

        private static bool isInitialized;

        public static void EnsureInitialized()
        {
            lock (syncRoot)
            {
                if (isInitialized)
                {
                    return;
                }

                ObjectFactory.Initialize(x =>
                    {
                        x.For<IGraphicsDeviceService>().Singleton().Use<GraphicsDeviceService>();
                        x.For<Texture2DService>().Singleton();
                    });
                isInitialized = true;
            }
        }
    }
}