namespace RedBadger.Xpf.Adapters.Xna
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
            var solidColorBrush = this.brush as SolidColorBrush;
            Rect drawRect = this.rect != Rect.Empty ? this.rect : new Rect();
            drawRect.X += offset.X;
            drawRect.Y += offset.Y;

            spriteBatch.Draw(this.texture2D, drawRect, solidColorBrush != null ? solidColorBrush.Color : Colors.Magenta);
        }
    }
}