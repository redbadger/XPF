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

            var grid = new Grid
                {
                    RowDefinitions = { new RowDefinition { Height = new GridLength(425) } },
                    ColumnDefinitions = { new ColumnDefinition { Width = new GridLength(400) } },
                    Background = new SolidColorBrush(Colors.Red),
                    Children =
                        {
                            new TextBlock(spriteFontAdapter)
                                {
                                    Text = "Your Score: 5483",
                                    Margin = new Thickness(5)
                                },
                            new TextBlock(spriteFontAdapter)
                                {
                                    Text = "High Score: 9999",
                                    Margin = new Thickness(5),
                                    HorizontalAlignment = HorizontalAlignment.Right 
                                },
                            new TextBlock(spriteFontAdapter)
                                {
                                    Text = "Lives: 3",
                                    Margin = new Thickness(5),
                                    VerticalAlignment = VerticalAlignment.Bottom 
                                }
                        }
                };

            this.rootElement.Content = grid;
        }
    }
}