namespace Xpf.Samples.S04BasketballScoreboard
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using Xpf.Samples.S04BasketballScoreboard.Domain;

    public class BasketballGame : Game
    {
        private Team homeTeam;

        private Team guestTeam;

        private Clock clock;

        public BasketballGame()
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
            this.homeTeam = new Team("HOME");
            this.guestTeam = new Team("GUEST");
            this.clock = new Clock();

            this.Components.Add(new ScoreboardView(this, this.homeTeam, this.guestTeam, this.clock));
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