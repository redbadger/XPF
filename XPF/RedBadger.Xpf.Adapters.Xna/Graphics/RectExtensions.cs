namespace RedBadger.Xpf.Adapters.Xna.Graphics
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     Extensions for XPF <see cref = "Rect">Rect</see>
    /// </summary>
    public static class RectExtensions
    {
        /// <summary>
        ///     Converts an XPF <see cref = "Rect">Rect</see> to an XNA <see cref = "Rectangle">Rectangle</see>.
        /// </summary>
        /// <param name = "rect">The XPF <see cref = "Rect">Rect</see> to convert.</param>
        /// <returns>The converted XNA <see cref = "Rectangle">Rectangle</see>.</returns>
        public static Rectangle ToRectangle(this Rect rect)
        {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }
    }
}