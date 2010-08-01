namespace RedBadger.Xpf.Presentation
{
    using System.Diagnostics;

    using Microsoft.Xna.Framework;

    [DebuggerDisplay("{Width} x {Height} @ {X}, {Y}")]
    public struct Rect
    {
        public float Height;

        public float Width;

        public float X;

        public float Y;

        public Rect(Size size)
            : this(Vector2.Zero, size)
        {
        }

        public Rect(Vector2 position, Size size)
            : this(position.X, position.Y, size.Width, size.Height)
        {
        }

        public Rect(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public static Rect Empty
        {
            get
            {
                return new Rect();
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this.X == 0f && this.Y == 0f && this.Width == 0f && this.Height == 0f;
            }
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