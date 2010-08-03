namespace RedBadger.PocketMechanic.Phone
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Controls;

    public class XpfTest : DrawableGameComponent
    {
        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatchAdapter;

        private SpriteFont spriteFont;

        public XpfTest(Game game)
            : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatchAdapter.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            this.rootElement.Draw(this.spriteBatchAdapter);
            this.spriteBatchAdapter.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.rootElement.Update();
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            this.spriteFont = this.Game.Content.Load<SpriteFont>("SpriteFont");
            this.spriteBatchAdapter = new SpriteBatchAdapter(this.GraphicsDevice);
            var spriteFontAdapter = new SpriteFontAdapter(this.spriteFont);

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(200) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(200) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(200) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(200) });

            var textBlock1 = new TextBlock(spriteFontAdapter) { Text = "TextBlock 1" };
            Grid.SetColumn(textBlock1, 0);
            grid.Children.Add(textBlock1);

            var textBlock2 = new TextBlock(spriteFontAdapter) { Text = "TextBlock 2" };
            Grid.SetColumn(textBlock2, 1);
            grid.Children.Add(textBlock2);

            var textBlock3 = new TextBlock(spriteFontAdapter) { Text = "TextBlock 3" };
            Grid.SetColumn(textBlock3, 0);
            Grid.SetRow(textBlock3, 1);
            grid.Children.Add(textBlock3);

            var textBlock4 = new TextBlock(spriteFontAdapter) { Text = "TextBlock 4" };
            Grid.SetColumn(textBlock4, 1);
            Grid.SetRow(textBlock4, 1);
            grid.Children.Add(textBlock4);

            var textBlock5 = new TextBlock(spriteFontAdapter) { Text = "TextBlock 5!" };
            Grid.SetColumn(textBlock5, 0);
            Grid.SetRow(textBlock5, 0);
            grid.Children.Add(textBlock5);

            var viewPort = new Rect(
                this.GraphicsDevice.Viewport.X, 
                this.GraphicsDevice.Viewport.Y, 
                this.GraphicsDevice.Viewport.Width, 
                this.GraphicsDevice.Viewport.Height);

            this.rootElement = new RootElement(viewPort) { Content = grid };
        }
    }
}