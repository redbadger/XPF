namespace XpfSamples.S02ApplicationBar
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;
    using RedBadger.Xpf.Presentation.Media.Imaging;

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

            var spriteFontAdapter = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("Segoe18"));
            var largeFont = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("Segoe30"));

            var addButtonImageTexture = new XnaImage(new Texture2DAdapter(this.Game.Content.Load<Texture2D>("AddButton")));
            var trashButtonImageTexture = new XnaImage(new Texture2DAdapter(this.Game.Content.Load<Texture2D>("TrashButton")));

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer);

            var grid = new Grid
                {
                    Background = new SolidColorBrush(Colors.Black), 
                    RowDefinitions = {
                                        new RowDefinition(), new RowDefinition { Height = new GridLength(70) } 
                                     }
                };

            var stackPanel = new StackPanel
                {
                    Children =
                        {
                            new TextBlock(spriteFontAdapter)
                                {
                                    Text = "MY APPLICATION", 
                                    Foreground = new SolidColorBrush(Colors.White), 
                                    Margin = new Thickness(10)
                                }, 
                            new TextBlock(largeFont)
                                {
                                    Text = "XNA Application Bar", 
                                    Foreground = new SolidColorBrush(Colors.White), 
                                    Margin = new Thickness(10)
                                }
                        }
                };
            grid.Children.Add(stackPanel);

            var applicationBar = new ApplicationBar
                {
                    Buttons =
                        {
                            new ApplicationBarIconButton(addButtonImageTexture), 
                            new ApplicationBarIconButton(trashButtonImageTexture)
                        }
                };

            Grid.SetRow(applicationBar, 1);
            grid.Children.Add(applicationBar);

            this.rootElement.Content = grid;
        }
    }
}