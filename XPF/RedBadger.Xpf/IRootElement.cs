#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

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
