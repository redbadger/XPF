namespace Xpf.Samples.S04BasketballScoreboard
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Court : DrawableGameComponent
    {
        private Matrix[] boneTransforms;

        private Model model;

        private Matrix projection;

        private Matrix view;

        public Court(Game game)
            : base(game)
        {
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

                    effect.View = this.view;
                    effect.Projection = this.projection;
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

            this.view = Matrix.CreateLookAt(new Vector3(50, 30, -80), new Vector3(0, 0, 50), Vector3.Up);
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 800 / 480f, .01f, 10000);
        }
    }
}