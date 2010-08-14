namespace RedBadger.Xpf.Presentation.Media
{
    using System.Windows;
    using System.Windows.Media;

    using RedBadger.Xpf.Graphics;

    using Vector = RedBadger.Xpf.Presentation.Vector;

    public struct SpriteTextureJob : ISpriteJob
    {
        private readonly Brush brush;

        private readonly ITexture2D texture2D;

        private Vector absoluteOffset;

        private Rect rect;

        public SpriteTextureJob(ITexture2D texture2D, Rect rect, Brush brush)
        {
            this.texture2D = texture2D;
            this.rect = rect;
            this.brush = brush;
            this.absoluteOffset = Vector.Zero;
        }

        public void Draw(ISpriteBatch spriteBatch)
        {
            var solidColorBrush = this.brush as SolidColorBrush;
            var drawRect = this.rect != Rect.Empty ? this.rect : new Rect();
            drawRect.X += this.absoluteOffset.X;
            drawRect.Y += this.absoluteOffset.Y;

            spriteBatch.Draw(this.texture2D, drawRect, solidColorBrush != null ? solidColorBrush.Color : Colors.Magenta);
        }

        public void SetAbsoluteOffset(Vector offset)
        {
            this.absoluteOffset = offset;
        }
    }
}