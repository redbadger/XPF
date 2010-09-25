namespace XpfSamples.S02ApplicationBar
{
    using System.Collections.ObjectModel;

    using Microsoft.Phone.Reactive;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf;
    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Adapters.Xna.Input;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Data;
    using RedBadger.Xpf.Media;
    using RedBadger.Xpf.Media.Imaging;

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
            this.rootElement.Draw();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.rootElement.Update();
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            this.spriteBatchAdapter = new SpriteBatchAdapter(new SpriteBatch(this.GraphicsDevice));
            var primitivesService = new PrimitivesService(this.GraphicsDevice);
            var renderer = new Renderer(this.spriteBatchAdapter, primitivesService);

            var spriteFontAdapter = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("Segoe18"));
            var largeFont = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("Segoe30"));

            var addButtonImageTexture =
                new TextureImage(new Texture2DAdapter(this.Game.Content.Load<Texture2D>("AddButton")));
            var trashButtonImageTexture =
                new TextureImage(new Texture2DAdapter(this.Game.Content.Load<Texture2D>("TrashButton")));

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager());

            var buttonClickResults = new ObservableCollection<string>();

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
                                }, 
                            new ItemsControl
                                {
                                    ItemsSource = buttonClickResults, 
                                    ItemTemplate = () =>
                                        {
                                            var textBlock = new TextBlock(spriteFontAdapter)
                                                {
                                                   Foreground = new SolidColorBrush(Colors.White) 
                                                };
                                            textBlock.Bind(
                                                TextBlock.TextProperty, BindingFactory.CreateOneWay<string>());
                                            return textBlock;
                                        }
                                }
                        }
                };
            grid.Children.Add(stackPanel);

            var applicationBar = new ApplicationBar
                {
                    Buttons =
                        {
                            new ApplicationBarIconButton("Add", addButtonImageTexture), 
                            new ApplicationBarIconButton("Delete", trashButtonImageTexture)
                        }
                };
            applicationBar.Clicks.Subscribe(
                Observer.Create<ApplicationBarIconButton>(s => buttonClickResults.Add(s.Text)));

            Grid.SetRow(applicationBar, 1);
            grid.Children.Add(applicationBar);

            this.rootElement.Content = grid;
        }
    }
}