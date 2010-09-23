namespace RedBadger.Xpf.Graphics
{
    /// <summary>
    ///     Represents a 2D Texture.
    /// </summary>
    public interface ITexture2D
    {
        /// <summary>
        ///     Gets the height of this texture in pixels.
        /// </summary>
        int Height { get; }

        /// <summary>
        ///     Gets the width of this texture in pixels.
        /// </summary>
        int Width { get; }
    }
}