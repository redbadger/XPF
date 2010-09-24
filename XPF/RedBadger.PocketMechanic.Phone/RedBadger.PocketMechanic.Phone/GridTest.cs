namespace RedBadger.PocketMechanic.Phone
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Adapters.Xna.Input;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Media;

    using Color = RedBadger.Xpf.Media.Color;

    public class GridTest : DrawableGameComponent
    {
        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatchAdapter;

        private SpriteFont spriteFont;

        public GridTest(Game game)
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
            this.spriteFont = this.Game.Content.Load<SpriteFont>("SpriteFont");
            this.spriteBatchAdapter = new SpriteBatchAdapter(new SpriteBatch(this.GraphicsDevice));
            var spriteFontAdapter = new SpriteFontAdapter(this.spriteFont);

            var renderer = new Renderer(this.spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice));
            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager());

            /*var border = new Border
                {
                    Width = 300,
                    Height = 300,
                    Background = new SolidColorBrush(Colors.DarkGray),
                    Child =
                    new StackPanel
                    {
                        Children = 
                        {
                            new TextBlock(spriteFontAdapter)
                            {
                                Text = "this can't all fit in the space sadsjds sd sd asd as das das da sd asd as dasd "
                            },
                            new Border
                                {
                                    Width = 100,
                                    Height = 100,
                                    Background = new SolidColorBrush(Colors.Cyan),
                                    Child = new TextBlock(spriteFontAdapter) { Text = "I wonder whether this will clip" }
                                },
                            new TextBlock(spriteFontAdapter)
                            {
                                Text = "this can't all fit in the space sadsjds sd sd asd as das das da sd asd as dasd "
                            }
                        }
                    }
                };

            this.rootElement.Content = border;*/


            var child_10_StackPanel = new StackPanel { Background = new SolidColorBrush(Colors.Red) };

            var child_20_Border = new Border { Width = 200, Height = 300, Background = new SolidColorBrush(Colors.Yellow) };
            var child_21_Border = new Border { Width = 100, Height = 100, Background = new SolidColorBrush(Colors.Blue) };

            var child_30_StackPanel = new StackPanel { Width = 300, Height = 400, Background = new SolidColorBrush(Colors.Green) };

            var child_40_Border = new Border { Width = 400, Height = 500, Background = new SolidColorBrush(Colors.Orange) };
            var child_41_Border = new Border { Width = 600, Height = 700, Background = new SolidColorBrush(Colors.Gray) };

            var child_50_Border = new Border { Width = 500, Height = 600, Background = new SolidColorBrush(Colors.Purple) };

            this.rootElement.Content = child_10_StackPanel;

            child_10_StackPanel.Children.Add(child_20_Border);
            child_10_StackPanel.Children.Add(child_21_Border);

            child_20_Border.Child = child_30_StackPanel;

            child_30_StackPanel.Children.Add(child_40_Border);
            child_30_StackPanel.Children.Add(child_41_Border);

            child_40_Border.Child = child_50_Border;
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