namespace RedBadger.Xpf.Media.Imaging
{
    public abstract class BitmapSource : ImageSource
    {
        public override double Height
        {
            get
            {
                return this.PixelHeight;
            }
        }

        public int PixelHeight { get; protected set; }

        public int PixelWidth { get; protected set; }

        public override double Width
        {
            get
            {
                return this.PixelWidth;
            }
        }
    }
}