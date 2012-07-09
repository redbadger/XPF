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

    public class Court : DrawableGameComponent
    {
        private readonly TouchCamera camera;

        private Matrix[] boneTransforms;

        private Model model;

        public Court(Game game, TouchCamera camera)
            : base(game)
        {
            this.camera = camera;
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (ModelMesh mesh in this.model.Meshes)
            {
                bool isTextured = mesh.Name.StartsWith("X");
                foreach (BasicEffect effect in mesh.Effects)
                {
                    Matrix transform = this.boneTransforms[mesh.ParentBone.Index];

                    effect.World = transform;

                    effect.View = this.camera.ViewMatrix;
                    effect.Projection = this.camera.ProjectionMatrix;
                    effect.DiffuseColor = Color.White.ToVector3();

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = false;
                    if (isTextured)
                    {
                        effect.TextureEnabled = true;
                    }
                }

                if (isTextured)
                {
                    this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            this.model = this.Game.Content.Load<Model>("court");
            this.boneTransforms = new Matrix[this.model.Bones.Count];
            this.model.CopyAbsoluteBoneTransformsTo(this.boneTransforms);
        }
    }
}
