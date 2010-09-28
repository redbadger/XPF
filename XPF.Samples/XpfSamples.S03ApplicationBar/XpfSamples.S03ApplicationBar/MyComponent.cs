namespace XpfSamples.S03ApplicationBar
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

            var header1 = new TextBlock(spriteFontAdapter)
                {
                    Text = "MY APPLICATION", 
                    Foreground = new SolidColorBrush(Colors.White), 
                    Margin = new Thickness(10)
                };
            var header2 = new TextBlock(largeFont)
                {
                    Text = "XNA Application Bar", 
                    Foreground = new SolidColorBrush(Colors.White), 
                    Margin = new Thickness(10)
                };
            var itemsControl = new ItemsControl
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
                };

            var scrollViewer = new ScrollViewer { Content = itemsControl };

            var applicationBar = new ApplicationBar
                {
                    Buttons =
                        {
                            new ApplicationBarIconButton("Add", addButtonImageTexture), 
                            new ApplicationBarIconButton("Delete", trashButtonImageTexture)
                        }
                };

            var grid = new Grid
                {
                    Background = new SolidColorBrush(Colors.Black), 
                    RowDefinitions =
                        {
                            new RowDefinition { Height = GridLength.Auto }, 
                            new RowDefinition { Height = GridLength.Auto }, 
                            new RowDefinition(), 
                            new RowDefinition { Height = new GridLength(70) }
                        }, 
                    Children =
                        {
                            header1, 
                            header2, 
                            scrollViewer,
                            applicationBar
                        }
                };

            applicationBar.Clicks.Subscribe(
                Observer.Create<ApplicationBarIconButton>(s => buttonClickResults.Add(s.Text)));

            Grid.SetRow(header1, 0);
            Grid.SetRow(header2, 1);
            Grid.SetRow(scrollViewer, 2);
            Grid.SetRow(applicationBar, 3);

            this.rootElement.Content = grid;
        }
    }
}