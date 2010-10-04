namespace Xpf.Samples.S04BasketballScoreboard
{
    using System;

    using Microsoft.Phone.Applications.Common;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Input.Touch;

    using Xpf.Samples.S04BasketballScoreboard.Domain;

    public class BasketballGame : Game
    {
        private const float ScoreTiltThreshold = 0.4f;

        private readonly object accelerometerLock = new object();

        private double accelerometerY;

        private TouchCamera camera;

        private Clock clock;

        private Team guestTeam;

        private Team homeTeam;

        private TimeSpan lastScored;

        public BasketballGame()
        {
            new GraphicsDeviceManager(this)
                {
                   SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight 
                };

            this.Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            this.TargetElapsedTime = TimeSpan.FromTicks(333333);
        }

        internal void ResetGraphicDeviceState()
        {
            this.GraphicsDevice.BlendState = BlendState.Opaque;
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
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

            this.camera = new TouchCamera { Bounds = new BoundingBox(new Vector3(-195), new Vector3(195)) };
            this.camera.LookAt(new Vector3(50, 50, -80), new Vector3(0, 0, 50), Vector3.Up);
            this.camera.Perspective(MathHelper.PiOver4, 800 / 480f, 1f, 600);

            var scoreboardView = new ScoreboardView(this, this.homeTeam, this.guestTeam, this.clock);
            this.Components.Add(scoreboardView);
            this.Components.Add(new ScoreboardQuad(this, this.camera, scoreboardView));
            this.Components.Add(new Court(this, this.camera));

            this.ResetGraphicDeviceState();
            TouchPanel.EnabledGestures = GestureType.FreeDrag | GestureType.Pinch;

            AccelerometerHelper.Instance.ReadingChanged += this.OnAccelerometerHelperReadingChanged;
            AccelerometerHelper.Instance.Active = true;

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            this.camera.Update(gameTime);
            this.UpdateScore();
            base.Update(gameTime);
        }

        private void OnAccelerometerHelperReadingChanged(object sender, AccelerometerHelperReadingEventArgs e)
        {
            lock (this.accelerometerLock)
            {
                this.accelerometerY = e.AverageAcceleration.Y;
            }
        }

        private void UpdateScore()
        {
            if (DateTime.Now.TimeOfDay.Subtract(this.lastScored).TotalSeconds > 1)
            {
                double y;
                lock (this.accelerometerLock)
                {
                    y = this.accelerometerY;
                }

                if (y < -ScoreTiltThreshold)
                {
                    this.homeTeam.IncrementScore(1);
                    this.lastScored = DateTime.Now.TimeOfDay;
                }
                else if (y > ScoreTiltThreshold)
                {
                    this.guestTeam.IncrementScore(1);
                    this.lastScored = DateTime.Now.TimeOfDay;
                }
            }
        }
    }
}