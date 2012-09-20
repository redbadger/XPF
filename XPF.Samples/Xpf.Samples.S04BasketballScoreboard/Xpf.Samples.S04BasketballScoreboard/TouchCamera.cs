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
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input.Touch;

    public class TouchCamera : Camera
    {
        private readonly Vector2 rotationSensitivity = new Vector2(5, 5);

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
