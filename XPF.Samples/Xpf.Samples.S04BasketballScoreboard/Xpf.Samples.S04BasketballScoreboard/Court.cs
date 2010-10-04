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
            this.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            foreach (ModelMesh mesh in this.model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    Matrix transform = this.boneTransforms[mesh.ParentBone.Index];

                    effect.World = transform;

                    effect.View = this.camera.ViewMatrix;
                    effect.Projection = this.camera.ProjectionMatrix;
                    effect.DiffuseColor = Color.White.ToVector3();

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = false;

                    effect.TextureEnabled = false;
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