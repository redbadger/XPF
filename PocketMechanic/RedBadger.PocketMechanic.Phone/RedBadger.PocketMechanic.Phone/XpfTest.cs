namespace RedBadger.PocketMechanic.Phone
{
    using System.Windows.Data;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;

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
            this.spriteFont = this.Game.Content.Load<SpriteFont>("SpriteFont");
            this.spriteBatchAdapter = new SpriteBatchAdapter(this.GraphicsDevice);
            var spriteFontAdapter = new SpriteFontAdapter(this.spriteFont);

            var grid = new Grid();
            var column1 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());


            var stackpanel = new StackPanel() { Orientation = Orientation.Horizontal };
            /*
            var sb = new Storyboard();
            var doubleAnimation = new DoubleAnimation()
                {
                    Duration = new Duration(TimeSpan.FromSeconds(4)),
                    From = 10,
                    To = 200,
                    RepeatBehavior = RepeatBehavior.Forever
                };
            Storyboard.SetTarget(doubleAnimation, column1);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("MaxWidth"));
            sb.Children.Add(doubleAnimation);
            sb.Begin();
*/
            var textBlock1 = new TextBlock(spriteFontAdapter);
            var border = new Border
                {
                    Child = textBlock1, 
                    BorderBrush = new SolidColorBrush(Color.Red), 
                    BorderThickness = new Thickness(20), 
                    Background = new SolidColorBrush(Color.Aquamarine)
                };

            Grid.SetColumn(border, 0);
            grid.Children.Add(border);
            //stackpanel.Children.Add(border);

            textBlock1.BindingFor(TextBlock.TextProperty).Is(new Binding("Time") { Source = new Clock() });

            var textBlock2 = new TextBlock(spriteFontAdapter) { Text = "Textblock 2" };
            border = new Border
                {
                    Child = textBlock2, 
                    BorderBrush = new SolidColorBrush(Color.Red), 
                    BorderThickness = new Thickness(20), 
                    Background = new SolidColorBrush(Color.Aquamarine)
                };
            Grid.SetColumn(border, 1);
            grid.Children.Add(border);
            //stackpanel.Children.Add(border);

            var textBlock3 = new TextBlock(spriteFontAdapter) { Text = "TextBlock 3" };
            Grid.SetColumn(textBlock3, 0);
            Grid.SetRow(textBlock3, 1);
            grid.Children.Add(textBlock3);

            var textBlock4 = new TextBlock(spriteFontAdapter) { Text = "TextBlock 4" };
            Grid.SetColumn(textBlock4, 1);
            Grid.SetRow(textBlock4, 1);
            grid.Children.Add(textBlock4);

            /*
            var textBlock5 = new TextBlock(spriteFontAdapter) { Text = "TextBlock 5!" };
            Grid.SetColumn(textBlock5, 0);
            Grid.SetRow(textBlock5, 0);
            grid.Children.Add(textBlock5);
*/
            var viewPort = new Rect(
                this.GraphicsDevice.Viewport.X, 
                this.GraphicsDevice.Viewport.Y, 
                this.GraphicsDevice.Viewport.Width, 
                this.GraphicsDevice.Viewport.Height);

            this.rootElement =
                new RootElement(
                    new Renderer(this.spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice)), viewPort)
                    {
                       Content = grid
                    };
        }
    }
}