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

namespace RedBadger.Xpf.Controls
{
    using System;
    using System.Linq;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Input;
    using RedBadger.Xpf.Media;

    /// <summary>
    ///     RootElement is the main host for all <see cref = "IElement">IElement</see>s, it manages the renderer, user input and is the target for Update/Draw calls.
    /// </summary>
    public class RootElement : ContentControl, IRootElement
    {
        /// <summary>
        ///     <see cref = "Viewport">Viewport</see> Reactive Property.
        /// </summary>
        public static readonly ReactiveProperty<Rect> ViewportProperty = ReactiveProperty<Rect>.Register(
            "Viewport", typeof(RootElement), ReactivePropertyChangedCallbacks.InvalidateMeasure);

        private readonly IInputManager inputManager;

        private readonly IRenderer renderer;

        private IElement elementWithMouseCapture;

        /// <summary>
        ///     Initializes a new instance of the <see cref = "RootElement">RootElement</see> class.
        /// </summary>
        /// <param name = "viewport">The viewport used to layout the <see cref = "RootElement">RootElement</see>'s content.</param>
        /// <param name = "renderer">An implementation of <see cref = "IRenderer">IRenderer</see> that can be used to render content.</param>
        public RootElement(Rect viewport, IRenderer renderer)
            : this(viewport, renderer, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref = "RootElement">RootElement</see> class.
        /// </summary>
        /// <param name = "viewport">The viewport used to layout the <see cref = "RootElement">RootElement</see>'s content.</param>
        /// <param name = "renderer">An implementation of <see cref = "IRenderer">IRenderer</see> that can be used to render content.</param>
        /// <param name = "inputManager">An implementation of <see cref = "IInputManager">IInputManager</see> that can be used to respond to user input.</param>
        public RootElement(Rect viewport, IRenderer renderer, IInputManager inputManager)
        {
            if (renderer == null)
            {
                throw new ArgumentNullException("renderer");
            }

            this.Viewport = viewport;
            this.renderer = renderer;
            this.inputManager = inputManager;

            if (this.inputManager != null)
            {
                this.inputManager.Gestures.Subscribe(this.Gestures);
            }
        }

        public IInputManager InputManager
        {
            get
            {
                return this.inputManager;
            }
        }

        public IRenderer Renderer
        {
            get
            {
                return this.renderer;
            }
        }

        /// <summary>
        ///     Gets or sets the viewport used by <see cref = "RootElement">RootElement</see> to layout its content.
        /// </summary>
        public Rect Viewport
        {
            get
            {
                return this.GetValue(ViewportProperty);
            }

            set
            {
                this.SetValue(ViewportProperty, value);
            }
        }

        /// <summary>
        ///     Draws a frame of XPF content.
        /// </summary>
        public void Draw()
        {
            this.renderer.Draw();
        }

        /// <summary>
        ///     Updates XPF layout logic.
        /// </summary>
        public void Update()
        {
            if (!this.IsArrangeValid)
            {
                this.renderer.ClearInvalidDrawingContexts();
            }

            Rect viewport = this.Viewport;
            this.Measure(new Size(viewport.Width, viewport.Height));
            this.Arrange(viewport);
            this.renderer.PreDraw();

            if (this.inputManager != null)
            {
                this.inputManager.Update();
            }
        }

        public bool CaptureMouse(IElement element)
        {
            if (this.elementWithMouseCapture == null)
            {
                this.elementWithMouseCapture = element;
                return true;
            }

            return false;
        }

        public void ReleaseMouseCapture(IElement element)
        {
            if (this.elementWithMouseCapture == element)
            {
                this.elementWithMouseCapture = null;
            }
        }

        protected override void OnNextGesture(Gesture gesture)
        {
            if (this.elementWithMouseCapture != null)
            {
                this.elementWithMouseCapture.Gestures.OnNext(gesture);
            }
            else
            {
                if (!OnNextGestureFindChild(this, gesture))
                {
                    OnNextGestureFindElement(this, gesture);
                }
            }
        }

        private static bool OnNextGestureFindChild(IElement element, Gesture gesture)
        {
            return
                element.GetVisualChildren().Reverse().Where(child => !OnNextGestureFindChild(child, gesture)).Any(
                    child => OnNextGestureFindElement(child, gesture));
        }

        private static bool OnNextGestureFindElement(IElement element, Gesture gesture)
        {
            if (element is IInputElement && element.HitTest(gesture.Point))
            {
                element.Gestures.OnNext(gesture);
                return true;
            }

            return false;
        }
    }
}
