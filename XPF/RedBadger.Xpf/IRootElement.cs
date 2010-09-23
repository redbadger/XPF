namespace RedBadger.Xpf
{
    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Input;
    using RedBadger.Xpf.Media;

    /// <summary>
    ///     <see cref = "IRootElement">IRootElement</see> is the main host for all <see cref = "IElement">IElement</see>s, it manages the renderer and user input.
    /// </summary>
    public interface IRootElement
    {
        /// <summary>
        ///     Gets the implementation of <see cref = "IInputManager">IInputManager</see> that is being used to respond to user input.
        /// </summary>
        IInputManager InputManager { get; }

        /// <summary>
        ///     Gets the implementation of <see cref = "IRenderer">IRenderer</see> that is being used to render content.
        /// </summary>
        IRenderer Renderer { get; }

        /// <summary>
        ///     Attempts to assign mouse capture to the specified element.
        /// </summary>
        /// <remarks>
        ///     <see cref = "IRootElement">IRootElement</see> acts as the central registry for mouse capture.  Only one element can have capture at a time and <see cref = "IRootElement">IRootElement</see> is responsible for ensuring that.
        /// </remarks>
        /// <param name = "element">The <see cref = "IElement">IElement</see> that is requesting mouse capture.</param>
        /// <returns>True if mouse capture was successful.</returns>
        bool CaptureMouse(IElement element);

        /// <summary>
        ///     Releases mouse capture for the specified element.
        /// </summary>
        /// <param name = "element">The <see cref = "IElement">IElement</see> that is releasing mouse capture.</param>
        void ReleaseMouseCapture(IElement element);
    }
}