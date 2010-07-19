namespace RedBadger.Xpf.Graphics
{
    using Microsoft.Xna.Framework.Graphics;

    public interface ITexture2D
    {
        int Height { get; }

        int Width { get; }

        Texture2D Value { get; }
    }
}
