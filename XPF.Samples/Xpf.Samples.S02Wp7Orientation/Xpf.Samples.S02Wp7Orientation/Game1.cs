namespace Xpf.Samples.S02Wp7Orientation
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class Game1 : Game
    {
        public Game1()
        {
            new GraphicsDeviceManager(this)
            {
                SupportedOrientations =
                    DisplayOrientation.Portrait | DisplayOrientation.LandscapeLeft |
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