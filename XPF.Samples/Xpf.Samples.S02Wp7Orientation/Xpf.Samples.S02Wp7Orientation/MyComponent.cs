namespace Xpf.Samples.S02Wp7Orientation
{
    using System;

    using Microsoft.Phone.Reactive;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf;
    using RedBadger.Xpf.Adapters.Xna;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Media;

    using Color = RedBadger.Xpf.Media.Color;

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

            Observable.FromEvent<EventArgs>(
                handler => this.Game.Window.OrientationChanged += handler, 
                handler => this.Game.Window.OrientationChanged -= handler).Subscribe(
                    _ => this.rootElement.Viewport = this.Game.GraphicsDevice.Viewport.ToRect());

            //// Alternative mechanism to hook up to the event.  Ensure you manage unhooking the event yourself.
            //// this.Game.Window.OrientationChanged += (sender, args) => this.rootElement.Viewport = this.Game.GraphicsDevice.Viewport.ToRect();
            var spriteFont = this.Game.Content.Load<SpriteFont>("MySpriteFont");
            var spriteFontAdapter = new SpriteFontAdapter(spriteFont);

            var grid = new Grid
                {
                    Background = new SolidColorBrush(Colors.White), 
                    RowDefinitions =
                        {
                            new RowDefinition { Height = new GridLength(50) }, 
                            new RowDefinition(), 
                            new RowDefinition { Height = new GridLength(50) }
                        }, 
                    ColumnDefinitions = {
                                           new ColumnDefinition(), new ColumnDefinition() 
                                        }
                };

            this.rootElement.Content = grid;

            var topLeftBorder = new Border
                {
                    BorderBrush = new SolidColorBrush(Colors.Black), 
                    BorderThickness = new Thickness(0, 0, 0, 2), 
                    Child = new TextBlock(spriteFontAdapter) { Text = "Score: 5483", Margin = new Thickness(10) }
                };
            Grid.SetRow(topLeftBorder, 0);
            Grid.SetColumn(topLeftBorder, 0);
            grid.Children.Add(topLeftBorder);

            var topRightBorder = new Border
                {
                    BorderBrush = new SolidColorBrush(Colors.Black), 
                    BorderThickness = new Thickness(0, 0, 0, 2), 
                    Child =
                        new TextBlock(spriteFontAdapter)
                            {
                                Text = "High: 9999", 
                                Margin = new Thickness(10), 
                                HorizontalAlignment = HorizontalAlignment.Right
                            }
                };
            Grid.SetRow(topRightBorder, 0);
            Grid.SetColumn(topRightBorder, 1);
            grid.Children.Add(topRightBorder);

            var bottomLeftBorder = new Border
                {
                    BorderBrush = new SolidColorBrush(Colors.Black), 
                    BorderThickness = new Thickness(0, 2, 0, 0), 
                    Background = new SolidColorBrush(new Color(106, 168, 79, 255)), 
                    Child =
                        new TextBlock(spriteFontAdapter)
                            {
                                Text = "Lives: 3", 
                                Margin = new Thickness(10), 
                                VerticalAlignment = VerticalAlignment.Bottom
                            }
                };
            Grid.SetRow(bottomLeftBorder, 2);
            Grid.SetColumn(bottomLeftBorder, 0);
            grid.Children.Add(bottomLeftBorder);

            var bottomRightBorder = new Border
                {
                    BorderBrush = new SolidColorBrush(Colors.Black), 
                    BorderThickness = new Thickness(0, 2, 0, 0), 
                    Background = new SolidColorBrush(new Color(106, 168, 79, 255))
                };
            Grid.SetRow(bottomRightBorder, 2);
            Grid.SetColumn(bottomRightBorder, 1);
            grid.Children.Add(bottomRightBorder);
        }
    }
}