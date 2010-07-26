namespace RedBadger.Xpf.Presentation.Controls
{
    using RedBadger.Xpf.Graphics;

    public class RootElement : ContentControl
    {
        private readonly Rect viewPort;

        public RootElement(Rect viewPort)
        {
            this.viewPort = viewPort;
        }

        public void Draw(ISpriteBatch spriteBatch)
        {
            this.Render(spriteBatch);
        }

        public void Update()
        {
            this.Measure(this.viewPort.Size);
            this.Arrange(this.viewPort);
        }
    }
}