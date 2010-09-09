namespace RedBadger.Xpf.Sandbox
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Input;
    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;

    using Color = Microsoft.Xna.Framework.Color;

    /// <summary>
    ///   This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private SpriteFontAdapter font;

        private GraphicsDeviceManager graphics;

        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatchAdapter;

        private SpriteFontAdapter spriteFontAdapter;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this)
                {
                   PreferredBackBufferWidth = 1024, PreferredBackBufferHeight = 768 
                };

            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatchAdapter.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            this.rootElement.Draw();
            this.spriteBatchAdapter.End();
            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            this.spriteFontAdapter = new SpriteFontAdapter(this.Content.Load<SpriteFont>("SpriteFont"));
            this.spriteBatchAdapter = new SpriteBatchAdapter(this.GraphicsDevice);

            var renderer = new Renderer(this.spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice));

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager());

            var itemsControl = new ItemsControl
                {
                    ItemsSource = new[] { "", "" },
                    ItemsPanel = new StackPanel { Orientation = Orientation.Horizontal},
                    ItemTemplate =
                        () =>
                        new Border
                            {
                                Background = new SolidColorBrush(Colors.Red),
                                Width = 100,
                                Height = 100,
                                Margin = new Thickness(10)
                            }
                };

            this.rootElement.Content = itemsControl;
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            this.rootElement.Update();
            base.Update(gameTime);
        }
    }
}