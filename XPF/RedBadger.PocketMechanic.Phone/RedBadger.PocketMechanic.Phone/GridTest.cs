namespace RedBadger.PocketMechanic.Phone
{
    using System;
    using System.Collections.ObjectModel;

    using Microsoft.Phone.Reactive;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Input;
    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Data;
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

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            var block = new TextBlock(spriteFontAdapter)
                {
                    Text = "Test Grid",
                    Background = new SolidColorBrush(GetRandomColor(this.random)),
                };
            grid.Children.Add(block);
            Grid.SetColumn(block, 1);
            Grid.SetRow(block, 1);
            

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