namespace RedBadger.Xpf.Sandbox
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Adapters.Xna.Input;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Media.Imaging;

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
            var primitiveService = new PrimitivesService(this.GraphicsDevice);
            var renderer = new Renderer(this.spriteBatch, primitiveService);

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager());

            this.circularMenu = new CircularMenu(this) { Width = 20, Height = 20 };

            this.rootElement.Content = this.circularMenu;
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name = "gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.B) && !this.lastState.IsKeyDown(Keys.B))
            {
                this.circularMenu.IsVisible = true;
            }

            if (!keyboardState.IsKeyDown(Keys.B) && this.lastState.IsKeyDown(Keys.B))
            {
                this.circularMenu.IsVisible = false;
            }

            this.lastState = keyboardState;
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