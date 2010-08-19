namespace RedBadger.PocketMechanic.Phone
{
    using System.Windows;
    using System.Windows.Media;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Input;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;
    using RedBadger.Xpf.Presentation.Media.Imaging;

    using SolidColorBrush = RedBadger.Xpf.Presentation.Media.SolidColorBrush;

    public class ScrollTest : DrawableGameComponent
    {
        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatchAdapter;

        private SpriteFont spriteFont;

        public ScrollTest(Game game)
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
            this.spriteFont = this.Game.Content.Load<SpriteFont>("SpriteFont");
            var badger3 = new XnaImage(new Texture2DAdapter(this.Game.Content.Load<Texture2D>("badger3")));
            this.spriteBatchAdapter = new SpriteBatchAdapter(this.GraphicsDevice);
            var spriteFontAdapter = new SpriteFontAdapter(this.spriteFont);

            var image = new Image { Source = badger3, Stretch = Stretch.UniformToFill };
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1000) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3000) });
            grid.Children.Add(image);

            var button = new Button
                {
                    Content =
                        new Border {
                               Background = new SolidColorBrush(Colors.Orange)
                            }, 
                    Width = 100, 
                    Height = 100
                };
            grid.Children.Add(button);

            var scrollViewer = new ScrollViewer { Content = grid };

            var viewPort = new Rect(
                this.GraphicsDevice.Viewport.X, 
                this.GraphicsDevice.Viewport.Y, 
                this.GraphicsDevice.Viewport.Width, 
                this.GraphicsDevice.Viewport.Height);

            var renderer = new Renderer(this.spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice));
            this.rootElement = new RootElement(viewPort, renderer, new InputManager()) { Content = scrollViewer };
        }
    }
}