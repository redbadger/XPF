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

namespace RedBadger.Xpf.Adapters.Xna.Input
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Subjects;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Input.Touch;
    using RedBadger.Xpf.Input;
    using GestureType = Microsoft.Xna.Framework.Input.Touch.GestureType;

    public class InputManager : IInputManager
    {
        private readonly Subject<Gesture> gestures = new Subject<Gesture>();

        private MouseState previousState;

        public InputManager()
        {
            TouchPanel.EnabledGestures |= GestureType.FreeDrag;
        }

        public IObservable<Gesture> Gestures
        {
            get
            {
                return this.gestures;
            }
        }

        public void Update()
        {
            MouseState currentState = Mouse.GetState();
            if (this.previousState.LeftButton == ButtonState.Released && currentState.LeftButton == ButtonState.Pressed)
            {
                this.gestures.OnNext(
                    new Gesture(Xpf.Input.GestureType.LeftButtonDown, new Point(currentState.X, currentState.Y)));
            }
            else if (this.previousState.LeftButton == ButtonState.Pressed &&
                     currentState.LeftButton == ButtonState.Released)
            {
                this.gestures.OnNext(
                    new Gesture(Xpf.Input.GestureType.LeftButtonUp, new Point(currentState.X, currentState.Y)));
            }

            if (currentState.X != this.previousState.X || currentState.Y != this.previousState.Y)
            {
                this.gestures.OnNext(new Gesture(Xpf.Input.GestureType.Move, new Point(currentState.X, currentState.Y)));

#if !WINDOWS_PHONE
                if (this.previousState.LeftButton == ButtonState.Pressed &&
                    currentState.LeftButton == ButtonState.Pressed)
                {
                    this.gestures.OnNext(
                        new Gesture(
                            Xpf.Input.GestureType.FreeDrag, 
                            new Point(currentState.X, currentState.Y), 
                            new Vector(currentState.X - this.previousState.X, currentState.Y - this.previousState.Y)));
                }
#endif
            }

            this.previousState = currentState;

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                switch (gesture.GestureType)
                {
                    case GestureType.FreeDrag:
                        this.gestures.OnNext(
                            new Gesture(
                                Xpf.Input.GestureType.FreeDrag, 
                                new Point(gesture.Position.X, gesture.Position.Y), 
                                new Vector(gesture.Delta.X, gesture.Delta.Y)));
                        break;
                }
            }
        }
    }
}
