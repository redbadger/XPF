namespace RedBadger.Xpf.Sandbox
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Adapters.Xna.Input;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Media;
    using RedBadger.Xpf.Media.Imaging;

    using Color = Microsoft.Xna.Framework.Color;

    public class MainGame : Game
    {
        private readonly GraphicsDeviceManager graphics;

        private CircularMenu circularMenu;

        private KeyboardState lastState;

        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatch;

        private TextureImage textureImage;

        public MainGame()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferHeight = 300;
            this.graphics.PreferredBackBufferWidth = 300;
            this.IsMouseVisible = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            this.rootElement.Draw();
            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatchAdapter(new SpriteBatch(this.GraphicsDevice));
            var renderer = new Renderer(this.spriteBatch, new PrimitivesService(this.GraphicsDevice));
            var spriteFontAdapter = new SpriteFontAdapter(this.Content.Load<SpriteFont>("SpriteFont"));

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager());

            var subject = new Subject<SolidColorBrush>();

            var firstItem = new Border
                {
                    BorderBrush = new SolidColorBrush(Colors.Black), 
                    BorderThickness = new Thickness(1, 1, 1, 1), 
                    Child = new TextBlock(spriteFontAdapter) { Text = "Party", Margin = new Thickness(10) }, 
                    HorizontalAlignment = HorizontalAlignment.Center, 
                    VerticalAlignment = VerticalAlignment.Center, 
                    Background = new SolidColorBrush(Colors.White)
                };
            firstItem.Bind(Border.BackgroundProperty, subject);

            var button = new Button
                {
                    Content =
                        new Border
                            {
                                Background = new SolidColorBrush(Colors.LightGray), 
                                Child = new TextBlock(spriteFontAdapter) { Text = "Change Color" }
                            }
                };
            button.Click += (sender, args) => subject.OnNext(new SolidColorBrush(Colors.Red));

            var stackPanel = new StackPanel { Children = { firstItem, button } };

            this.rootElement.Content = stackPanel;
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

    public class CircularMenu : ContentControl
    {
        private readonly TextureImage textureImage;

        private bool isVisible;

        public CircularMenu(Game game)
        {
            this.textureImage = new TextureImage(new Texture2DAdapter(game.Content.Load<Texture2D>("bulb")));
        }

        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }

            set
            {
                if (this.isVisible != value)
                {
                    this.isVisible = value;
                    this.Content = this.isVisible ? new Image { Source = this.textureImage } : null;
                }
            }
        }
    }
}