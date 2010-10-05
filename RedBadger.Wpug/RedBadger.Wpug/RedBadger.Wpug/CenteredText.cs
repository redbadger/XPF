namespace RedBadger.Wpug
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf;
    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Media;
    using RedBadger.Xpf.Media.Imaging;

    public class CenteredText : DrawableGameComponent
    {
        private RootElement rootElement;

        public CenteredText(Game game)
            : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            this.rootElement.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            this.rootElement.Draw();
        }

        protected override void LoadContent()
        {
            var spriteBatchAdapter = new SpriteBatchAdapter(new SpriteBatch(this.GraphicsDevice));
            var spriteFontAdapter = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("SpriteFont"));

            var renderer = new Renderer(spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice));

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer);

            var logo = new TextureImage(new Texture2DAdapter(this.Game.Content.Load<Texture2D>("WP7Logos")));
            var stackPanel = new StackPanel
                {
                    Children =
                        {
                            new TextBlock(spriteFontAdapter) { Text = "Windows Phone User Group", Margin = new Thickness(20) },
                            new Image { Source = logo, Stretch = Stretch.None }
                        },
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

            this.rootElement.Content = stackPanel;

            this.Game.Window.OrientationChanged += this.OnOrientationChanged;
        }

        private void OnOrientationChanged(object sender, EventArgs eventArgs)
        {
            this.rootElement.Viewport = this.GraphicsDevice.Viewport.ToRect();
        }
    }
}