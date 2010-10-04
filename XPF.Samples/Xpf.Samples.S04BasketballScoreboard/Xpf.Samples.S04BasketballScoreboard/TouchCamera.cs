namespace Xpf.Samples.S04BasketballScoreboard
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input.Touch;

    public class TouchCamera : Camera
    {
        private readonly Vector2 rotationSensitivity = new Vector2(10, 10);

        private Vector3 direction;

        public TouchCamera()
            : base("Touch Camera")
        {
        }

        public void Update(GameTime gameTime)
        {
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
                            float delta = currentGap - previousGap;

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

            this.Move(this.direction);
        }
    }
}