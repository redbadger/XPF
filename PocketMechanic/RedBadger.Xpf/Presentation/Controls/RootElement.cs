namespace RedBadger.Xpf.Presentation.Controls
{
    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation.Media;

    public class RootElement : ContentControl
    {
        private readonly Rect viewPort;

        private bool isFirst = true;

        public RootElement(Rect viewPort)
        {
            this.viewPort = viewPort;
        }

        public void Draw(ISpriteBatch spriteBatch)
        {
            XpfServiceLocator.Get<DrawingState>().Draw(spriteBatch);
        }

        public void Update()
        {
            this.Measure(this.viewPort.Size);
            this.Arrange(this.viewPort);

            if (this.isFirst)
            {
                XpfServiceLocator.Get<DrawingState>().ResolveOffsets();
                this.isFirst = false;
            }
        }
    }
}