namespace RedBadger.Xpf.Presentation.Media.Imaging
{
    public abstract class BitmapSource : ImageSource
    {
        public override float Height
        {
            get
            {
                return this.PixelHeight;
            }
        }

        public int PixelHeight { get; protected set; }

        public int PixelWidth { get; protected set; }

        public override float Width
        {
            get
            {
                return this.PixelWidth;
            }
        }
    }
}