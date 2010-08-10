namespace RedBadger.Xpf.Presentation.Media
{
    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;

    public struct SpriteTextureJob : ISpriteJob
    {
        private readonly Brush brush;

        private readonly Rect rect;

        private readonly ITexture2D texture2D;

        public SpriteTextureJob(ITexture2D texture2D, Rect rect, Brush brush)
        {
            this.texture2D = texture2D;
            this.rect = rect;
            this.brush = brush;
        }

        public void Draw(ISpriteBatch spriteBatch)
        {
            var solidColorBrush = this.brush as SolidColorBrush;
            spriteBatch.Draw(this.texture2D, this.rect, solidColorBrush != null ? solidColorBrush.Color : Color.Purple);
        }
    }
}