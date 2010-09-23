namespace RedBadger.Xpf.Graphics
{
    /// <summary>
    ///     Represents a Font Texture.
    /// </summary>
    public interface ISpriteFont
    {
        /// <summary>
        ///     Measures the given text for this instance of <see cref = "ISpriteFont">ISpriteFont</see>.
        /// </summary>
        /// <param name = "text">The text to measure.</param>
        /// <returns>The <see cref = "Size">Size</see> of the text.</returns>
        Size MeasureString(string text);
    }
}