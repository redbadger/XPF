#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

namespace Xpf.Samples.S04BasketballScoreboard
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Input.Touch;

    using Xpf.Samples.S04BasketballScoreboard.Accelerometer;
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
                    SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight, 
                    IsFullScreen = true
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
            this.camera.LookAt(new Vector3(135, 40, 0), new Vector3(200, 40, 0), Vector3.Up);
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
