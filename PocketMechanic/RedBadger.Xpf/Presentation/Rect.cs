namespace RedBadger.Xpf.Presentation
{
    using Microsoft.Xna.Framework;

    public struct Rect
    {
        public float Height;

        public float Width;

        public float X;

        public float Y;

        public Rect(Vector2 position, Size size) : this(position.X, position.Y, size.Width, size.Height)
        {
        }

        public Rect(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public Vector2 Position
        {
            get
            {
                return new Vector2(this.X, this.Y);
            }
        }

        public Size Size
        {
            get
            {
                return new Size(this.Width, this.Height);
            }
        }
    }
}