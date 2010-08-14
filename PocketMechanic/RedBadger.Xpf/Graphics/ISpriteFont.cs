namespace RedBadger.Xpf.Graphics
{
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Presentation;

    public interface ISpriteFont
    {
        SpriteFont Value
        {
            get;
        }

        Size MeasureString(string text);
    }
}
