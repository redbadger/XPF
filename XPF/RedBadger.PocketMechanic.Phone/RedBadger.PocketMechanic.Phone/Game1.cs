namespace RedBadger.PocketMechanic.Phone
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using RedBadger.Xpf.Sandbox;

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

            this.Components.Add(new MyComponent(this));
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
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