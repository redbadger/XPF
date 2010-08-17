namespace RedBadger.Xpf.Presentation.Input
{
    using System;

    public interface IInputManager
    {
        IObservable<MouseData> MouseData { get; }

        void Update();
    }
}