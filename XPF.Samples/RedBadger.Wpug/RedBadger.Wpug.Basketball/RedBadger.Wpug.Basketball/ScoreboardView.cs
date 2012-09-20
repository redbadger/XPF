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

namespace RedBadger.Wpug.Basketball
{
    using System;

    using Microsoft.Phone.Reactive;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf;
    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Media;

    public class ScoreboardView : DrawableGameComponent
    {
        private SpriteFontAdapter lcd;

        private SpriteFontAdapter led;

        private RootElement rootElement;

        public ScoreboardView(BasketballGame game)
            : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            this.rootElement.Draw();
        }

        public override void Update(GameTime gameTime)
        {
            this.rootElement.Update();
        }

        protected override void LoadContent()
        {
            var spriteBatchAdapter = new SpriteBatchAdapter(new SpriteBatch(this.GraphicsDevice));
            var renderer = new Renderer(spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice));
            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer);

            this.lcd = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("Lcd"));
            this.led = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("Led"));

            Observable.FromEvent<EventArgs>(
                handler => this.Game.Window.OrientationChanged += handler, 
                handler => this.Game.Window.OrientationChanged -= handler).Subscribe(
                    _ => this.rootElement.Viewport = this.Game.GraphicsDevice.Viewport.ToRect());

            IElement homeTeamPanel = this.CreateTeamDisplay();

            var clockPanel = new StackPanel
                {
                    Children =
                        {
                            new Border
                                {
                                    HorizontalAlignment = HorizontalAlignment.Center, 
                                    BorderBrush = new SolidColorBrush(Colors.White), 
                                    BorderThickness = new Thickness(4), 
                                    Padding = new Thickness(10), 
                                    Margin = new Thickness(10), 
                                    Child =
                                        new TextBlock(this.led)
                                            {
                                                Text = "00:00", 
                                                Foreground = new SolidColorBrush(Colors.Red), 
                                                HorizontalAlignment = HorizontalAlignment.Center
                                            }
                                }, 
                            new StackPanel
                                {
                                    HorizontalAlignment = HorizontalAlignment.Center, 
                                    Orientation = Orientation.Horizontal, 
                                    Children =
                                        {
                                            new TextBlock(this.lcd)
                                                {
                                                    Text = "PERIOD", 
                                                    Foreground = new SolidColorBrush(Colors.LightGray), 
                                                    Padding = new Thickness(10), 
                                                    VerticalAlignment = VerticalAlignment.Center
                                                }, 
                                            new TextBlock(this.led)
                                                {
                                                    Text = "0", 
                                                    Foreground = new SolidColorBrush(Colors.Yellow), 
                                                    Padding = new Thickness(10), 
                                                    VerticalAlignment = VerticalAlignment.Center
                                                }
                                        }
                                }
                        }
                };

            IElement guestTeamPanel = this.CreateTeamDisplay();

            var grid = new Grid
                {
                    Background = new SolidColorBrush(Colors.Black), 
                    ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = GridLength.Auto }, 
                            new ColumnDefinition(), 
                            new ColumnDefinition { Width = GridLength.Auto }
                        }, 
                    Children = {
                                   homeTeamPanel, clockPanel, guestTeamPanel 
                               }
                };

            Grid.SetColumn(homeTeamPanel, 0);
            Grid.SetColumn(clockPanel, 1);
            Grid.SetColumn(guestTeamPanel, 2);
            var border = new Border
                {
                    VerticalAlignment = VerticalAlignment.Top, 
                    BorderBrush = new SolidColorBrush(Colors.LightGray), 
                    BorderThickness = new Thickness(5), 
                    Child = grid, 
                };

            this.rootElement.Content = border;
        }

        private IElement CreateTeamDisplay()
        {
            var teamNameTextBlock = new TextBlock(this.lcd)
                {
                    Text = "Team", 
                    Foreground = new SolidColorBrush(Colors.LightGray), 
                    HorizontalAlignment = HorizontalAlignment.Center, 
                    Padding = new Thickness(25)
                };

            var scoreTextBlock = new TextBlock(this.led)
                {
                    Text = "0", 
                    Foreground = new SolidColorBrush(Colors.Green), 
                    HorizontalAlignment = HorizontalAlignment.Center
                };

            return new StackPanel { Children = { teamNameTextBlock, scoreTextBlock } };
        }
    }
}
