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
    using Microsoft.Xna.Framework.Graphics;

    public class ScoreboardQuad : DrawableGameComponent
    {
        private readonly BasketballGame basketballGame;

        private readonly TouchCamera camera;

        private readonly ScoreboardView scoreboardView;

        private Quad quad;

        private BasicEffect quadEffect;

        private RenderTarget2D scoreboardTexture;

        public ScoreboardQuad(BasketballGame basketballGame, TouchCamera camera, ScoreboardView scoreboardView)
            : base(basketballGame)
        {
            this.basketballGame = basketballGame;
            this.camera = camera;
            this.scoreboardView = scoreboardView;
        }

        public override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.SetRenderTarget(this.scoreboardTexture);
            this.scoreboardView.Draw(gameTime);
            this.GraphicsDevice.SetRenderTarget(null);

            this.basketballGame.ResetGraphicDeviceState();

            this.quadEffect.View = this.camera.ViewMatrix;
            this.quadEffect.Projection = this.camera.ProjectionMatrix;
            this.quadEffect.Texture = this.scoreboardTexture;

            this.quadEffect.World = Matrix.CreateRotationY(MathHelper.ToRadians(90)) *
                                    Matrix.CreateTranslation(198, 50, 0);

            foreach (EffectPass pass in this.quadEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                this.GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList, this.quad.Vertices, 0, 4, this.quad.Indices, 0, 2);
            }

            this.quadEffect.World = Matrix.CreateRotationY(MathHelper.ToRadians(-90)) *
                                    Matrix.CreateTranslation(-198, 50, 0);

            foreach (EffectPass pass in this.quadEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                this.GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList, this.quad.Vertices, 0, 4, this.quad.Indices, 0, 2);
            }
        }

        public override void Initialize()
        {
            this.quad = new Quad(Vector3.Zero, Vector3.Forward, Vector3.Up, 80, 28);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.scoreboardTexture = new RenderTarget2D(this.GraphicsDevice, 800, 280);

            this.quadEffect = new BasicEffect(this.GraphicsDevice);
            this.quadEffect.EnableDefaultLighting();

            this.quadEffect.World = Matrix.Identity;
            this.quadEffect.TextureEnabled = true;
        }
    }
}
