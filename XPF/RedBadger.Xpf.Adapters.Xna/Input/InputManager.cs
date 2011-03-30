namespace RedBadger.Xpf.Adapters.Xna.Input
{
    using System;
    using System.Collections.Generic;

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