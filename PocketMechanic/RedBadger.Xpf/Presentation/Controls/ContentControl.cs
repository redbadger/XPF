namespace RedBadger.Xpf.Presentation.Controls
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Represents a control with a single piece of content.
    /// </summary>
    public class ContentControl : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentControl"/> class.
        /// </summary>
        public ContentControl()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentControl"/> class.
        /// </summary>
        /// <param name="drawPosition">The draw position.  If this is a child control, drawPosition will not be honoured.</param>
        public ContentControl(Vector2 drawPosition)
        {
            this.DrawPosition = drawPosition;
        }

        public IElement Content { get; set; }

        protected override void DrawImplementation()
        {
            this.Content.Draw();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (this.Content != null)
            {
                this.Content.Measure(availableSize);
                return this.Content.DesiredSize;
            }
            
            return Size.Empty;
        }
    }
}