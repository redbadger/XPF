namespace RedBadger.Xpf
{
    using Ninject;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation.Media;

    public static class XpfServiceLocator
    {
        private static readonly IKernel kernel = new StandardKernel();

        static XpfServiceLocator()
        {
            kernel.Bind<IRenderer>().To<Renderer>().InSingletonScope();
        }

        public static void RegisterPrimitiveService(IPrimitivesService primitivesService)
        {
            kernel.Bind<IPrimitivesService>().ToMethod(context => primitivesService);
        }

        internal static T Get<T>()
        {
            return kernel.Get<T>();
        }
    }
}