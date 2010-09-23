namespace RedBadger.Xpf.Graphics
{
    using RedBadger.Xpf.Media;

    /// <summary>
    ///     A batch of Sprites that should be sent to the GPU for rendering together.
    /// </summary>
    public interface ISpriteBatch
    {
        /// <summary>
        ///     Adds a <see cref = "ITexture2D">ITexture2D</see> sprite to the batch for rendering.
        /// </summary>
        /// <param name = "texture2D">The <see cref="ITexture2D">ITexture2D</see> to render.</param>
        /// <param name = "rect">A <see cref="Rect">Rect</see> defining the destination in pixels for drawing the <see cref = "ITexture2D">ITexture2D</see>.</param>
        /// <param name = "color">The <see cref = "Color">Color</see> to tint the <see cref = "ITexture2D">ITexture2D</see>.</param>
        void Draw(ITexture2D texture2D, Rect rect, Color color);

        /// <summary>
        ///     Adds a <see cref = "ISpriteFont">ISpriteFont</see> to the batch for rendering.
        /// </summary>
        /// <param name = "spriteFont">The <see cref = "ISpriteFont">ISpriteFont</see> to render.</param>
        /// <param name = "text">The text to render.</param>
        /// <param name = "position">The position in pixels to render the text.</param>
        /// <param name = "color">The <see cref = "Color">Color</see> to tint the <see cref = "ISpriteFont">ISpriteFont</see>.</param>
        void DrawString(ISpriteFont spriteFont, string text, Vector position, Color color);
    }
}