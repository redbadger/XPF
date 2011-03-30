namespace RedBadger.Xpf.Sandbox
{
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Adapters.Xna.Input;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Media;
    using RedBadger.Xpf.Media.Imaging;

    public class MyComponent : DrawableGameComponent
    {
        private RootElement rootElement;

        public MyComponent(Game game)
            : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            this.rootElement.Draw();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.rootElement.Update();
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            var spriteBatchAdapter = new SpriteBatchAdapter(new SpriteBatch(this.GraphicsDevice));
            var primitivesService = new PrimitivesService(this.GraphicsDevice);
            var renderer = new Renderer(spriteBatchAdapter, primitivesService);
            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager());
            var scrollView = new ScrollViewer();
            this.rootElement.Content = scrollView;
            var stackPanel = new StackPanel();
            scrollView.Content = stackPanel;
            scrollView.CanHorizontallyScroll = true;
            var headerFont = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("SpriteFont"));
            var descFont = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("SpriteFont"));

            foreach (int app in Enumerable.Range(0, 2))
            {
                var grid = new Grid
                {
                    ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Auto }, new ColumnDefinition() }
                };

                grid.Children.Add(
                    new Image
                    {
                        Source =
                            new TextureImage(
                            new Texture2DAdapter(this.Game.Content.Load<Texture2D>("Textures/badger2")))
                    });

                var textStack = new StackPanel { Margin = new Thickness(12, 0, 0, 0) };
                Grid.SetColumn(textStack, 1);
                grid.Children.Add(textStack);

                textStack.Children.Add(new TextBlock(headerFont) { Text = "Title" });
                textStack.Children.Add(
                    new TextBlock(descFont)
                    {
                        Text = "Description - some text that should wrap when it runs of the screen.",
                        Wrapping = TextWrapping.Wrap,
                    });

                stackPanel.Children.Add(grid);
            }
        }
    }

    public class App
    {
        public string Description { get; set; }

        public string Title { get; set; }
    }
}