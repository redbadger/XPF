namespace RedBadger.Xpf.Input
{
    using System;

    public interface IInputManager
    {
        IObservable<Gesture> Gestures { get; }

        void Update();
    }
}