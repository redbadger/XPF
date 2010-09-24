namespace RedBadger.Xpf.Graphics
{
    /// <summary>
    ///     Provides primitives that XPF requires to render correctly.
    /// </summary>
    public interface IPrimitivesService
    {
        /// <summary>
        ///     A single pixel that XPF can use to draw lines, borders and backgrounds.
        /// </summary>
        ITexture SinglePixel { get; }
    }
}