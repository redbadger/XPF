namespace RedBadger.Xpf.Graphics
{
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Presentation;

    public static class ViewportExtensions
    {
        public static Rect ToRect(this Viewport viewport)
        {
            return new Rect(viewport.X, viewport.Y, viewport.Width, viewport.Height);
        }
    }
}