namespace Xpf.Samples.S04BasketballScoreboard
{
    using System;
    using System.Diagnostics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input.Touch;

    public class TouchCamera : CameraBehaviour
    {
        private readonly Vector3 defaultAcceleration = new Vector3(50);

        private readonly Vector3 defaultVelocity = new Vector3(100);

        private readonly Vector2 rotationSensitivity = new Vector2(8, 8);

        private Vector3 acceleration;

        private Vector3 currentVelocity;

        private Vector3 direction;

        private Vector3 velocity;

        public TouchCamera(Game game)
            : base("Spectator Camera")
        {
            this.acceleration = this.defaultAcceleration;
            this.velocity = this.defaultVelocity;

            /*Rectangle clientBounds = game.Window.ClientBounds;
            float aspect = (float)clientBounds.Width / clientBounds.Height;*/
            this.ApplyConfiguration();
        }

        public void Update(GameTime gameTime)
        {
            var elapsedTimeSec = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TouchPanel.IsGestureAvailable)
            {
                while (TouchPanel.IsGestureAvailable)
                {
                    GestureSample gesture = TouchPanel.ReadGesture();

                    switch (gesture.GestureType)
                    {
                        case GestureType.Pinch:
                            float previousGap = Vector2.Distance(gesture.Position - gesture.Delta, gesture.Position2 - gesture.Delta2);
                            float currentGap = Vector2.Distance(gesture.Position, gesture.Position2);
                            float delta = (currentGap - previousGap) * 100;

                            Debug.WriteLine(delta);
                            this.direction = new Vector3(0, 0, delta);
                            break;
                        case GestureType.FreeDrag:
                            var rotation = new Vector2(gesture.Delta.X, gesture.Delta.Y);
                            rotation /= this.rotationSensitivity;
                            this.Rotate(rotation.X, rotation.Y, 0f);
                            break;
                    }
                }
            }
            else
            {
                this.direction = Vector3.Zero;
            }

            this.UpdateCameraTranslation(elapsedTimeSec);
        }

        private void ApplyConfiguration()
        {
            /*this.LookAt(new Vector3(0, -600, -800), Vector3.Zero, Vector3.Up);
            this.Perspective(MathHelper.PiOver4, 800 / 480f, 1, 5000);*/

            this.LookAt(new Vector3(50, 30, -80), new Vector3(0, 0, 50), Vector3.Up);
            this.Perspective(MathHelper.PiOver4, 800 / 480f, .01f, 10000);
        }

        /// <summary>
        ///     Determines which way to move the camera based on player input.
        ///     The returned values are in the range [-1,1].
        /// </summary>
        private void GetMovementDirection()
        {
            /*this.direction = this.playerControls.Move;*/
        }

        private void UpdateCameraTranslation(float elapsedTimeSec)
        {
            this.GetMovementDirection();

            this.UpdatePosition(elapsedTimeSec);
        }

        /// <summary>
        ///     Moves the camera based on player input.
        /// </summary>
        /// <param name = "elapsedTimeSec">Elapsed game time.</param>
        private void UpdatePosition(float elapsedTimeSec)
        {

            Debug.WriteLine(this.currentVelocity);

            if (this.currentVelocity.LengthSquared() != 0.0f)
            {
                // Only move the camera if the velocity vector is not of zero
                // length. Doing this guards against the camera slowly creeping
                // around due to floating point rounding errors.
                Vector3 displacement = this.currentVelocity * elapsedTimeSec;

                // Floating point rounding errors will slowly accumulate and
                // cause the camera to move along each axis. To prevent any
                // unintended movement the displacement vector is clamped to
                // zero for each direction that the camera isn't moving in.
                // The UpdateVelocity() method will slowly decelerate
                // the camera's velocity back to a stationary state when the
                // camera is no longer moving along that direction. To account
                // for this the camera's current velocity is also checked.
                if (this.direction.X == 0.0f && Math.Abs(this.currentVelocity.X) < 1e-6f)
                {
                    displacement.X = 0.0f;
                }

                if (this.direction.Y == 0.0f && Math.Abs(this.currentVelocity.Y) < 1e-6f)
                {
                    displacement.Y = 0.0f;
                }

                if (this.direction.Z == 0.0f && Math.Abs(this.currentVelocity.Z) < 1e-6f)
                {
                    displacement.Z = 0.0f;
                }

                this.Move(displacement);
            }

            // Continuously update the camera's velocity vector even if the
            // camera hasn't moved during this call. When the camera is no
            // longer being moved the camera is decelerating back to its
            // stationary state.
            this.UpdateVelocity(elapsedTimeSec);
        }

        /// <summary>
        ///     Updates the camera's velocity based on the supplied movement
        ///     direction and the elapsed time (since this method was last
        ///     called). The movement direction is the in the range [-1,1].
        /// </summary>
        /// <param name = "elapsedTimeSec">Elapsed game time.</param>
        private void UpdateVelocity(float elapsedTimeSec)
        {
            if (this.direction.X != 0.0f)
            {
                // Camera is moving along the x axis.
                // Linearly accelerate up to the camera's max speed.
                this.currentVelocity.X += this.direction.X * this.acceleration.X * elapsedTimeSec;

                if (this.currentVelocity.X > this.velocity.X)
                {
                    this.currentVelocity.X = this.velocity.X;
                }
                else if (this.currentVelocity.X < -this.velocity.X)
                {
                    this.currentVelocity.X = -this.velocity.X;
                }
            }
            else
            {
                // Camera is no longer moving along the x axis.
                // Linearly decelerate back to stationary state.
                if (this.currentVelocity.X > 0.0f)
                {
                    if ((this.currentVelocity.X -= this.acceleration.X * elapsedTimeSec) < 0.0f)
                    {
                        this.currentVelocity.X = 0.0f;
                    }
                }
                else
                {
                    if ((this.currentVelocity.X += this.acceleration.X * elapsedTimeSec) > 0.0f)
                    {
                        this.currentVelocity.X = 0.0f;
                    }
                }
            }

            if (this.direction.Y != 0.0f)
            {
                // Camera is moving along the y axis.
                // Linearly accelerate up to the camera's max speed.
                this.currentVelocity.Y += this.direction.Y * this.acceleration.Y * elapsedTimeSec;

                if (this.currentVelocity.Y > this.velocity.Y)
                {
                    this.currentVelocity.Y = this.velocity.Y;
                }
                else if (this.currentVelocity.Y < -this.velocity.Y)
                {
                    this.currentVelocity.Y = -this.velocity.Y;
                }
            }
            else
            {
                // Camera is no longer moving along the y axis.
                // Linearly decelerate back to stationary state.
                if (this.currentVelocity.Y > 0.0f)
                {
                    if ((this.currentVelocity.Y -= this.acceleration.Y * elapsedTimeSec) < 0.0f)
                    {
                        this.currentVelocity.Y = 0.0f;
                    }
                }
                else
                {
                    if ((this.currentVelocity.Y += this.acceleration.Y * elapsedTimeSec) > 0.0f)
                    {
                        this.currentVelocity.Y = 0.0f;
                    }
                }
            }

            if (this.direction.Z != 0.0f)
            {
                // Camera is moving along the z axis.
                // Linearly accelerate up to the camera's max speed.
                this.currentVelocity.Z += this.direction.Z * this.acceleration.Z * elapsedTimeSec;

                if (this.currentVelocity.Z > this.velocity.Z)
                {
                    this.currentVelocity.Z = this.velocity.Z;
                }
                else if (this.currentVelocity.Z < -this.velocity.Z)
                {
                    this.currentVelocity.Z = -this.velocity.Z;
                }
            }
            else
            {
                // Camera is no longer moving along the z axis.
                // Linearly decelerate back to stationary state.
                if (this.currentVelocity.Z > 0.0f)
                {
                    if ((this.currentVelocity.Z -= this.acceleration.Z * elapsedTimeSec) < 0.0f)
                    {
                        this.currentVelocity.Z = 0.0f;
                    }
                }
                else
                {
                    if ((this.currentVelocity.Z += this.acceleration.Z * elapsedTimeSec) > 0.0f)
                    {
                        this.currentVelocity.Z = 0.0f;
                    }
                }
            }
        }
    }
}