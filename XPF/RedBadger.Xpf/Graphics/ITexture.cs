namespace RedBadger.Xpf.Graphics
{
    /// <summary>
    ///     Represents a Texture.
    /// </summary>
    public interface ITexture
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