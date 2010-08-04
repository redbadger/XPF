namespace RedBadger.Xpf.Presentation.Media
{
    using System.Windows;

    using Size = RedBadger.Xpf.Presentation.Size;

    public abstract class ImageSource : DependencyObject
    {
        public abstract float Height { get; }

        public abstract float Width { get; }

        internal virtual Size Size
        {
            get
            {
                return new Size(this.Width, this.Height);
            }
        }
    }
}