namespace RedBadger.Xpf.Adapters.Xna.Graphics
{
    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Media;

    public struct SpriteTextureJob : ISpriteJob
    {
        private readonly Brush brush;

        private readonly Rect rect;

        private readonly ITexture texture;

        public SpriteTextureJob(ITexture texture, Rect rect, Brush brush)
        {
            this.texture = texture;
            this.rect = rect;
            this.brush = brush;
        }

        public void Draw(ISpriteBatch spriteBatch, Vector offset)
        {
            Rect drawRect = !this.rect.IsEmpty ? this.rect : new Rect();
            drawRect.Displace(offset);

            var solidColorBrush = this.brush as SolidColorBrush;
            spriteBatch.Draw(this.texture, drawRect, solidColorBrush != null ? solidColorBrush.Color : Colors.Magenta);
        }
    }
}