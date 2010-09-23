namespace RedBadger.Xpf.Adapters.Xna.Graphics
{
    using Microsoft.Xna.Framework.Graphics;

    public static class ViewportExtensions
    {
        public static Rect ToRect(this Viewport viewport)
        {
            return new Rect(viewport.X, viewport.Y, viewport.Width, viewport.Height);
        }
    }
}