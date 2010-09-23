namespace RedBadger.PocketMechanic.Phone
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Adapters.Xna;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;

    using Color = RedBadger.Xpf.Presentation.Media.Color;

    public class GridTest : DrawableGameComponent
    {
        private readonly Random random = new Random();

        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatchAdapter;

        private SpriteFont spriteFont;

        private StackPanel stackPanel;

        private TextBlock textBlock;

        public GridTest(Game game)
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

            var grid = new Grid { Width = 200, Height = 200 };
            grid.ColumnDefinitions.Add(new ColumnDefinition { MinWidth = 30d });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition { MinWidth = 100d });

            var child1 = new Border { Background = new SolidColorBrush(Colors.Red) };
            var child2 = new Border { Background = new SolidColorBrush(Colors.Yellow) };
            var child3 = new Border { Background = new SolidColorBrush(Colors.Blue) };
            Grid.SetColumn(child1, 0);
            Grid.SetColumn(child2, 1);
            Grid.SetColumn(child3, 2);

            grid.Children.Add(child1);
            grid.Children.Add(child2);
            grid.Children.Add(child3);

            child2.Width = 100d;

            var renderer = new Renderer(this.spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice));

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager())
                {
                   Content = grid 
                };
        }

        private static Color GetRandomColor(Random random)
        {
            int next = random.Next(3);
            switch (next)
            {
                case 0:
                    return Colors.Red;
                case 1:
                    return Colors.Blue;
                case 2:
                    return Colors.Yellow;
            }

            return Colors.Purple;
        }
    }
}