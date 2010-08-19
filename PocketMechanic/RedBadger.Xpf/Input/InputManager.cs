namespace RedBadger.Xpf.Input
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Microsoft.Xna.Framework.Input;

    using RedBadger.Xpf.Presentation.Input;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    public class InputManager : IInputManager
    {
        private readonly Subject<Gesture> gestures = new Subject<Gesture>();

        private MouseState previousState;

        public IObservable<Gesture> Gestures
        {
            get
            {
                return this.gestures;
            }
        }

        public void Update()
        {
            var currentState = Mouse.GetState();
            if (this.previousState.LeftButton == ButtonState.Released && currentState.LeftButton == ButtonState.Pressed)
            {
                this.gestures.OnNext(
                    new Gesture(GestureType.LeftButtonDown, new Point(currentState.X, currentState.Y)));
            }
            else if (this.previousState.LeftButton == ButtonState.Pressed &&
                     currentState.LeftButton == ButtonState.Released)
            {
                this.gestures.OnNext(
                    new Gesture(GestureType.LeftButtonUp, new Point(currentState.X, currentState.Y)));
            }

            if (currentState.X != this.previousState.X || currentState.Y != this.previousState.Y)
            {
                this.gestures.OnNext(new Gesture(GestureType.Move, new Point(currentState.X, currentState.Y)));
            }

            this.previousState = currentState;
        }
    }
}