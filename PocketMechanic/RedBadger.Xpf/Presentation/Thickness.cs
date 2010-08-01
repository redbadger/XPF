namespace RedBadger.Xpf.Presentation
{
    using System.Diagnostics;

    [DebuggerDisplay("{Left}, {Top}, {Right}, {Bottom}")]
    public struct Thickness
    {
        public float Bottom;
        public float Left;
        public float Right;
        public float Top;

        public Thickness(float left, float top)
            : this(left, top, left, top)
        {
        }

        public Thickness(float uniformLength)
            : this(uniformLength, uniformLength, uniformLength, uniformLength)
        {
        }

        public Thickness(float left, float top, float right, float bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }
    }
}