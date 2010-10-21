namespace Xpf.Samples.S05DataBinding101
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf;
    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Adapters.Xna.Input;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Controls.Primitives;
    using RedBadger.Xpf.Data;
    using RedBadger.Xpf.Media;
    using RedBadger.Xpf.Media.Imaging;

    public class MyComponent : DrawableGameComponent
    {
        private RootElement rootElement;

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
            var spriteBatchAdapter = new SpriteBatchAdapter(new SpriteBatch(this.GraphicsDevice));
            var spriteFontAdapter = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("SpriteFont"));
            var renderer = new Renderer(spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice));
            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager());

            // Setup Layout
            var cardImage = new Image
                {
                    Stretch = Stretch.None
                };

            var cardToggleButton = new ToggleButton
                {
                    Content = cardImage,
                    Margin = new Thickness(10)
                };

            var resetButton = new Button
                {
                    Content =
                        new Border
                            {
                                Background = new SolidColorBrush(Colors.LightGray), 
                                Child = new TextBlock(spriteFontAdapter)
                                    {
                                        Text = "Reset",
                                        Margin = new Thickness(10)
                                    }
                            }, 
                    Margin = new Thickness(10), 
                    HorizontalAlignment = HorizontalAlignment.Center
                };

            var stackPanel = new StackPanel
                {
                    Children =
                        {
                            cardToggleButton,
                            resetButton
                        }
                };

            this.rootElement.Content = stackPanel;

            // Setup Data Binding
            var faceDownImage = new TextureImage(new Texture2DAdapter(this.Game.Content.Load<Texture2D>("FaceDown")));
            var faceUpImage = new TextureImage(new Texture2DAdapter(this.Game.Content.Load<Texture2D>("FaceUp")));

            var card = new Card(faceDownImage, faceUpImage);

            cardImage.Bind(
                Image.SourceProperty,
                BindingFactory.CreateOneWay<Card, ImageSource>(card, d => d.CardImage));

            cardToggleButton.Bind(
                ToggleButton.IsCheckedProperty,
                BindingFactory.CreateTwoWay(card, d => d.IsCardFaceUp));

            resetButton.Click += (sender, args) => card.Reset();
        }
    }
}