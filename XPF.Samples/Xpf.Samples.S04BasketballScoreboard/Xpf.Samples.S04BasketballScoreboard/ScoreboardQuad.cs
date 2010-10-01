namespace Xpf.Samples.S04BasketballScoreboard
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class ScoreboardQuad : DrawableGameComponent
    {
        private readonly Matrix projection;

        private readonly ScoreboardView scoreboardView;

        private readonly Matrix view;

        private Quad quad;

        private BasicEffect quadEffect;

        private RenderTarget2D scoreboardTexture;

        public ScoreboardQuad(Game game, Matrix view, Matrix projection, ScoreboardView scoreboardView)
            : base(game)
        {
            this.view = view;
            this.projection = projection;
            this.scoreboardView = scoreboardView;
        }

        public override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.SetRenderTarget(this.scoreboardTexture);
            this.scoreboardView.Draw(gameTime);
            this.GraphicsDevice.SetRenderTarget(null);

            this.quadEffect.Texture = this.scoreboardTexture;

            foreach (EffectPass pass in this.quadEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                this.GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList, this.quad.Vertices, 0, 4, this.quad.Indices, 0, 2);
            }
        }

        public override void Initialize()
        {
            this.quad = new Quad(Vector3.Zero, Vector3.Forward, Vector3.Up, 800, 350);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.scoreboardTexture = new RenderTarget2D(this.GraphicsDevice, 800, 350);

            this.quadEffect = new BasicEffect(this.GraphicsDevice);
            this.quadEffect.EnableDefaultLighting();

            this.quadEffect.World = Matrix.Identity;
            this.quadEffect.View = this.view;
            this.quadEffect.Projection = this.projection;
            this.quadEffect.TextureEnabled = true;
        }
    }
}