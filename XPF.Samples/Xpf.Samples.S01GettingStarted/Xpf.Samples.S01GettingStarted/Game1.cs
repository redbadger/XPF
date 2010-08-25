namespace Xpf.Samples.S01GettingStarted
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 400;
            this.graphics.PreferredBackBufferHeight = 425;
            this.Content.RootDirectory = "Content";
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            base.Update(gameTime);
        }
    }
}