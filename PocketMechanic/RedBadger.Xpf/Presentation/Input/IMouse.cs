namespace RedBadger.Xpf.Presentation.Input
{
    using System;
    using System.Windows;

    public interface IMouse
    {
        IObservable<MouseData> MouseData { get; }

        void Update();
    }

    public struct MouseData
    {
        public MouseAction Action;

        public Point Point;
    }

    public enum MouseAction
    {
        LeftButtonDown
    }
}