namespace RedBadger.Xpf.Sandbox
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation.Controls;

    /// <summary>
    ///   This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private SpriteFontAdapter font;

        private GraphicsDeviceManager graphics;

        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatchAdapter;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this)
                {
                   PreferredBackBufferWidth = 1024, PreferredBackBufferHeight = 768 
                };

            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatchAdapter.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            this.rootElement.Draw();
            this.spriteBatchAdapter.End();
            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            this.rootElement.Update();
            base.Update(gameTime);
        }
    }
}