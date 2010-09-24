namespace RedBadger.Xpf.Adapters.Xna.Graphics
{
    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Media;

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

        public void Draw(ISpriteBatch spriteBatch, Vector offset)
        {
            Rect drawRect = !this.rect.IsEmpty ? this.rect : new Rect();
            drawRect.Displace(offset);

            var solidColorBrush = this.brush as SolidColorBrush;
            spriteBatch.Draw(this.texture2D, drawRect, solidColorBrush != null ? solidColorBrush.Color : Colors.Magenta);
        }
    }
}