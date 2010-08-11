namespace RedBadger.Xpf.Presentation.Media
{
    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Graphics;

    public struct SpriteTextureJob : ISpriteJob
    {
        private readonly Brush brush;

        private readonly ITexture2D texture2D;

        private Vector2 absoluteOffset;

        private Rect rect;

        public SpriteTextureJob(ITexture2D texture2D, Rect rect, Brush brush)
        {
            this.texture2D = texture2D;
            this.rect = rect;
            this.brush = brush;
            this.absoluteOffset = Vector2.Zero;
        }

        public void Draw(ISpriteBatch spriteBatch)
        {
            var solidColorBrush = this.brush as SolidColorBrush;
            var drawRect = new Rect(this.rect.Position + this.absoluteOffset, this.rect.Size);

            spriteBatch.Draw(this.texture2D, drawRect, solidColorBrush != null ? solidColorBrush.Color : Color.Maroon);
        }

        public void SetAbsoluteOffset(Vector2 offset)
        {
            this.absoluteOffset = offset;
        }
    }
}