namespace XpfSamples.S03ApplicationBar
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = 480;
            this.graphics.PreferredBackBufferHeight = 800;

            // Frame rate is 30 fps by default for Windows Phone.
            this.TargetElapsedTime = TimeSpan.FromTicks(333333);
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

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            // TODO: Add your update logic here
            base.Update(gameTime);
        }
    }
}