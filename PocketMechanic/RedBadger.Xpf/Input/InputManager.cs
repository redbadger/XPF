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
        private readonly Subject<MouseData> mouseData = new Subject<MouseData>();

        private MouseState previousState;

        public IObservable<MouseData> MouseData
        {
            get
            {
                return this.mouseData;
            }
        }

        public void Update()
        {
            var currentState = Mouse.GetState();
            if (this.previousState.LeftButton == ButtonState.Released && currentState.LeftButton == ButtonState.Pressed)
            {
                this.mouseData.OnNext(
                    new MouseData(MouseAction.LeftButtonDown, new Point(currentState.X, currentState.Y)));
            }
            else if (this.previousState.LeftButton == ButtonState.Pressed &&
                     currentState.LeftButton == ButtonState.Released)
            {
                this.mouseData.OnNext(
                    new MouseData(MouseAction.LeftButtonUp, new Point(currentState.X, currentState.Y)));
            }

            if (currentState.X != this.previousState.X || currentState.Y != this.previousState.Y)
            {
                this.mouseData.OnNext(new MouseData(MouseAction.Move, new Point(currentState.X, currentState.Y)));
            }

            this.previousState = currentState;
        }
    }
}