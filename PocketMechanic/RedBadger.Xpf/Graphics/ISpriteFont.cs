namespace RedBadger.Xpf.Graphics
{
    using System.Windows;

    using Microsoft.Xna.Framework.Graphics;

    public interface ISpriteFont
    {
        SpriteFont Value
        {
            get;
        }

        Size MeasureString(string text);
    }
}
