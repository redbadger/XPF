namespace RedBadger.Xpf.Presentation.Input
{
    using System;

    public interface IInputManager
    {
        IObservable<Gesture> Gestures { get; }

        void Update();
    }
}