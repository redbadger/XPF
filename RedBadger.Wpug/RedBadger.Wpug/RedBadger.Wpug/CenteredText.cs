namespace RedBadger.Wpug
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class CenteredText : DrawableGameComponent
    {
        private Vector2 drawPosition;

        private SpriteBatch spriteBatch;

        private SpriteFont spriteFont;

        private string text;

        public CenteredText(Game game)
            : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            this.spriteBatch.DrawString(this.spriteFont, this.text, this.drawPosition, Color.Black);

            this.spriteBatch.End();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.spriteFont = this.Game.Content.Load<SpriteFont>("SpriteFont");

            this.text = "Windows Phone User Group";

            Viewport viewport = this.GraphicsDevice.Viewport;
            Vector2 measureString = this.spriteFont.MeasureString(this.text);

            this.drawPosition.X = (viewport.Width / 2f) - (measureString.X / 2f);
            this.drawPosition.Y = (viewport.Height / 2f) - (measureString.Y / 2f);
        }
    }
}