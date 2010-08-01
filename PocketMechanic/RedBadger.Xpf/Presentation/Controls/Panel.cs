namespace RedBadger.Xpf.Presentation.Controls
{
    using RedBadger.Xpf.Graphics;

    public class Panel : UIElement
    {
        private readonly UIElementCollection children;

        public Panel()
        {
            this.children = new UIElementCollection(this);
        }

        public UIElementCollection Children
        {
            get
            {
                return this.children;
            }
        }

        public override void Render(ISpriteBatch spriteBatch)
        {
        }
    }
}