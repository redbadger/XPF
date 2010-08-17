namespace RedBadger.Xpf.Presentation.Input
{
    using System;
    using System.Windows;

    public class MouseButtonEventArgs : EventArgs
    {
        private readonly Point position;

        public MouseButtonEventArgs(Point position)
        {
            this.position = position;
        }

        public Point Position
        {
            get
            {
                return this.position;
            }
        }
    }
}