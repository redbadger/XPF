namespace RedBadger.Xpf.Sandbox
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class MainGame : Game
    {
        public MainGame()
        {
            new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }

        protected override void Initialize()
        {
            this.Components.Add(new MyComponent(this));
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            base.Update(gameTime);
        }
    }
}