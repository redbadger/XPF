namespace Xpf.Samples.S04BasketballScoreboard
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using Xpf.Samples.S04BasketballScoreboard.Domain;

    public class BasketballGame : Game
    {
        private Clock clock;

        private Team guestTeam;

        private Team homeTeam;

        private Matrix projection;

        private Matrix view;

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

            this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
        }

        protected override void Initialize()
        {
            this.homeTeam = new Team("HOME");
            this.guestTeam = new Team("GUEST");
            this.clock = new Clock();

            this.view = Matrix.CreateLookAt(new Vector3(0, -600, -800), Vector3.Zero, Vector3.Up);
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 800 / 480f, 1, 5000);

/*
            var scoreboardView = new ScoreboardView(this, this.homeTeam, this.guestTeam, this.clock);
            this.Components.Add(scoreboardView);
            this.Components.Add(new ScoreboardQuad(this, this.view, this.projection, scoreboardView));
*/
            this.Components.Add(new Court(this));

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