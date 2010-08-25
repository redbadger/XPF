namespace Xpf.Samples.S01GettingStarted
{
    using System.Windows;
    using System.Windows.Media;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;

    using SolidColorBrush = RedBadger.Xpf.Presentation.Media.SolidColorBrush;
    using TextWrapping = RedBadger.Xpf.Presentation.TextWrapping;

    public class MyComponent : DrawableGameComponent
    {
        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatchAdapter;

        public MyComponent(Game game)
            : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatchAdapter.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            this.rootElement.Draw();
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
            this.spriteBatchAdapter = new SpriteBatchAdapter(this.GraphicsDevice);
            var primitivesService = new PrimitivesService(this.GraphicsDevice);
            var renderer = new Renderer(this.spriteBatchAdapter, primitivesService);

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer);

            var spriteFont = this.Game.Content.Load<SpriteFont>("MySpriteFont");
            var spriteFontAdapter = new SpriteFontAdapter(spriteFont);

            var textBlock = new TextBlock(spriteFontAdapter)
                {
                    Text =
                        "Red Badger is a product and service consultancy, specialising in bespoke software projects, developer tools and platforms on the Microsoft technology stack.", 
                    Background = new SolidColorBrush(Colors.Red), 
                    HorizontalAlignment = HorizontalAlignment.Left, 
                    VerticalAlignment = VerticalAlignment.Top, 
                    Wrapping = TextWrapping.Wrap, 
                    Margin = new Thickness(10)
                };

            this.rootElement.Content = textBlock;

            var stackPanel = new StackPanel
                {
                    Background = new SolidColorBrush(Colors.Red), 
                    Children =
                        {
                            new TextBlock(spriteFontAdapter) { Text = "Item 1" }, 
                            new TextBlock(spriteFontAdapter) { Text = "Item 2" }, 
                            new TextBlock(spriteFontAdapter) { Text = "Item 3" }
                        }, 
                    HorizontalAlignment = HorizontalAlignment.Left, 
                    VerticalAlignment = VerticalAlignment.Top, 
                    Orientation = Orientation.Horizontal, 
                    Margin = new Thickness(10)
                };

            this.rootElement.Content = stackPanel;

            /*var grid = new Grid
                {
                    RowDefinitions = { new RowDefinition(), new RowDefinition() },
                    ColumnDefinitions = { new ColumnDefinition(), new ColumnDefinition() }
                };
            this.rootElement.Content = grid;*/
        }
    }
}