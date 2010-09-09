namespace RedBadger.PocketMechanic.Phone
{
    using System.ComponentModel;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Input;
    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;

    public class XpfTest : DrawableGameComponent
    {
        private SpriteFontAdapter spriteFontAdapter;

        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatchAdapter;

        public XpfTest(Game game)
            : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatchAdapter.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            this.rootElement.Draw();
            this.spriteBatchAdapter.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.rootElement.Update();
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            this.spriteFontAdapter = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("SpriteFont"));
            this.spriteBatchAdapter = new SpriteBatchAdapter(this.GraphicsDevice);

            var renderer = new Renderer(this.spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice));

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager());

            var itemsControl = new ItemsControl();

            // this.rootElement.Content = itemsControl;
        }
    }
}