namespace RedBadger.Xpf.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface ISpriteFont
    {
        SpriteFont Value
        {
            get;
        }

        Vector2 MeasureString(string text);
    }
}
