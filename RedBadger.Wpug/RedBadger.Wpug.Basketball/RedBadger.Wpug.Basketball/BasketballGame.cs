namespace RedBadger.Wpug.Basketball
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class BasketballGame : Game
    {
        private GraphicsDeviceManager graphics;

        public BasketballGame()
        {
            this.graphics = new GraphicsDeviceManager(this)
            {
                SupportedOrientations =
                    DisplayOrientation.LandscapeLeft |
                    DisplayOrientation.LandscapeRight
            };

            this.Content.RootDirectory = "Content";

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
            this.Components.Add(new ScoreboardView(this));
            base.Initialize();
        }

        protected override void LoadContent()
        {
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