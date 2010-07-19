namespace RedBadger.Xpf.Presentation
{
    public struct Size
    {
        public float Width;

        public float Height;

        public Size(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        public static Size Empty
        {
            get
            {
                return new Size();
            }
        }

        public static Size operator +(Size value1, Size value2)
        {
            Size newSize;
            newSize.Width = value1.Width + value2.Width;
            newSize.Height = value1.Height + value2.Height;

            return newSize;
        }

        public override string ToString()
        {
            return string.Format("Width: {0}, Height: {1}", this.Width, this.Height);
        }
    }
}