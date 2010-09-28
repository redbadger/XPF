using RedBadger.Xpf;
using RedBadger.Xpf.Input;

namespace NekoCake.Crimson.Xpf
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Input.Touch;
    using RedBadger.Xpf;
    using RedBadger.Xpf.Input;
    using GestureType = RedBadger.Xpf.Input.GestureType;
    using XnaGestureType = Microsoft.Xna.Framework.Input.Touch.GestureType;

    public class InputManagerWindows : IInputManager
    {
        // Fields
        private readonly Subject<Gesture> gestures = new Subject<Gesture>();
        private MouseState previousState;

        // Methods
        public InputManagerWindows()
        {
            TouchPanel.EnabledGestures = XnaGestureType.FreeDrag;
        }

        public void Update()
        {
            MouseState state = Mouse.GetState();
            if ((this.previousState.LeftButton == ButtonState.Released) && (state.LeftButton == ButtonState.Pressed))
            {
                this.gestures.OnNext(new Gesture(GestureType.LeftButtonDown, new Point((double)state.X, (double)state.Y)));
            }
            else if ((this.previousState.LeftButton == ButtonState.Pressed) && (state.LeftButton == ButtonState.Released))
            {
                this.gestures.OnNext(new Gesture(GestureType.LeftButtonUp, new Point((double)state.X, (double)state.Y)));
            }
            if ((state.X != this.previousState.X) || (state.Y != this.previousState.Y))
            {
                this.gestures.OnNext(new Gesture(GestureType.Move, new Point((double)state.X, (double)state.Y)));
            }
            if ((this.previousState.LeftButton == ButtonState.Pressed) && (state.LeftButton == ButtonState.Pressed))
            {
                this.gestures.OnNext(new Gesture(GestureType.FreeDrag, new Point((double)state.X, (double)state.Y), new Vector(state.X - previousState.X, state.Y - previousState.Y)));
            }

            this.previousState = state;
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample sample = TouchPanel.ReadGesture();
                if (sample.GestureType == XnaGestureType.FreeDrag)
                {
                    this.gestures.OnNext(new Gesture(GestureType.FreeDrag, new Point((double)sample.Position.X, (double)sample.Position.Y), new Vector((double)sample.Delta.X, (double)sample.Delta.Y)));
                }
            }
        }

        // Properties
        public IObservable<Gesture> Gestures
        {
            get
            {
                return this.gestures;
            }
        }
    }
}
