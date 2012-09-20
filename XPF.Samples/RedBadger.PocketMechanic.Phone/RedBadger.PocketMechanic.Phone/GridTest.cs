#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

namespace RedBadger.PocketMechanic.Phone
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf;
    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Adapters.Xna.Input;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Media;

    using Color = RedBadger.Xpf.Media.Color;
    using Point = Microsoft.Xna.Framework.Point;

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

            var border = new Border
                {
                    Width = 300,
                    Height = 300,
                    Background = new SolidColorBrush(Colors.DarkGray),
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(2)
                };

            var outerBorder = new Border
                {
                    Background = new SolidColorBrush(Colors.Yellow),
                    BorderBrush = new SolidColorBrush(Colors.Red),
                    BorderThickness = new Thickness(2),
                    Child = border
                };

            var button = new Button
                {
                    Content = new Border { Background = new SolidColorBrush(Colors.Brown), Width = 100, Height = 50 }
                };
            button.Click += (sender, args) =>
                {
                    if (border.Width == 300)
                    {
                        border.Width = 100;
                        border.Height = 150;
                    }
                    else
                    {
                        border.Width = 300;
                        border.Height = 300;
                    }
                };

            var stackPanel = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Children = { outerBorder, button }
                };

            this.rootElement.Content = stackPanel;

            /*var child_10_StackPanel = new StackPanel { Background = new SolidColorBrush(Colors.Red) };

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

            child_40_Border.Child = child_50_Border;*/
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
