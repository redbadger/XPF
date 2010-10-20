namespace RedBadger.Xpf.Sandbox
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Data;
    using RedBadger.Xpf.Media;

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
            var primitivesService = new PrimitivesService(this.GraphicsDevice);
            var renderer = new Renderer(spriteBatchAdapter, primitivesService);

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer);

            var colors = new List<SolidColorBrush> {
                    new SolidColorBrush(Colors.Red), 
                    new SolidColorBrush(Colors.Blue), 
                    new SolidColorBrush(Colors.Yellow)
                };

            var itemsControl = new ItemsControl
            {
                ItemsPanel = new StackPanel { Orientation = Orientation.Horizontal },
                ItemsSource = colors,
                ItemTemplate = _ =>
                {
                    var border = new Border
                    {
                        Width = 50,
                        Height = 50,
                        Margin = new Thickness(10)
                    };

                    border.Bind(
                        Border.BackgroundProperty,
                        BindingFactory.CreateOneWay<Brush>());

                    return border;
                }
            }; 
            
            this.rootElement.Content = itemsControl;
        }
    }
}