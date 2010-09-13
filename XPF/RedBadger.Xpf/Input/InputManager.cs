namespace RedBadger.Xpf.Input
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Input.Touch;

    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Input;

    using GestureType = Microsoft.Xna.Framework.Input.Touch.GestureType;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    public class InputManager : IInputManager
    {
        private readonly Subject<Gesture> gestures = new Subject<Gesture>();

        private MouseState previousState;

        public InputManager()
        {
            TouchPanel.EnabledGestures = GestureType.FreeDrag;
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
                    new Gesture(
                        Presentation.Input.GestureType.LeftButtonDown, new Point(currentState.X, currentState.Y)));
            }
            else if (this.previousState.LeftButton == ButtonState.Pressed &&
                     currentState.LeftButton == ButtonState.Released)
            {
                this.gestures.OnNext(
                    new Gesture(
                        Presentation.Input.GestureType.LeftButtonUp, new Point(currentState.X, currentState.Y)));
            }

            if (currentState.X != this.previousState.X || currentState.Y != this.previousState.Y)
            {
                this.gestures.OnNext(
                    new Gesture(Presentation.Input.GestureType.Move, new Point(currentState.X, currentState.Y)));
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
                                Presentation.Input.GestureType.FreeDrag, 
                                new Point(gesture.Position.X, gesture.Position.Y), 
                                new Vector(gesture.Delta.X, gesture.Delta.Y)));
                        break;
                }
            }
        }
    }
}