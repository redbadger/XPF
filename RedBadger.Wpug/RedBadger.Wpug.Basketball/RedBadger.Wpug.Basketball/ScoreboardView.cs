namespace RedBadger.Wpug.Basketball
{
    using System;

    using Microsoft.Phone.Reactive;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Wpug.Basketball.Domain;
    using RedBadger.Xpf;
    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Adapters.Xna.Input;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Data;
    using RedBadger.Xpf.Media;

    public class ScoreboardView : DrawableGameComponent
    {
        private SpriteFontAdapter largeLabel;

        private SpriteFontAdapter largeLed;

        private RootElement rootElement;

        private SpriteFontAdapter basicFont;

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
            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager());

            var smallLabel = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("SmallLabel"));
            this.largeLabel = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("SmallLabel"));

            var smallLed = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("SmallLed"));
            this.largeLed = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("LargeLed"));

            this.basicFont = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("BasicSpriteFont"));

            Observable.FromEvent<EventArgs>(
                handler => this.Game.Window.OrientationChanged += handler,
                handler => this.Game.Window.OrientationChanged -= handler).Subscribe(
                    _ => this.rootElement.Viewport = this.Game.GraphicsDevice.Viewport.ToRect());

            var timeTextBlock = new TextBlock(this.largeLed)
                {
                    Foreground = new SolidColorBrush(Colors.Red),
                    HorizontalAlignment = HorizontalAlignment.Center
                };

            var clock = new Clock();
            var homeTeam = new Team("HOME");
            var guestTeam = new Team("GUEST");

            timeTextBlock.Bind(TextBlock.TextProperty, clock.TimeDisplay);

            var periodTextBlock = new TextBlock(smallLed)
                {
                    Foreground = new SolidColorBrush(Colors.Yellow),
                    Padding = new Thickness(10)
                };
            periodTextBlock.Bind(
                TextBlock.TextProperty, BindingFactory.CreateOneWay<Clock, int, string>(clock, c => c.Period));

            IElement homeTeamPanel = this.CreateTeamDisplay(homeTeam);

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
                                    Width = 220, 
                                    Child = timeTextBlock
                                }, 
                            new StackPanel
                                {
                                    HorizontalAlignment = HorizontalAlignment.Center, 
                                    Orientation = Orientation.Horizontal, 
                                    Children =
                                        {
                                            new TextBlock(smallLabel)
                                                {
                                                    Text = "PERIOD", 
                                                    Foreground = new SolidColorBrush(Colors.White), 
                                                    Padding = new Thickness(10)
                                                }, 
                                            periodTextBlock
                                        }
                                }
                        }
                };

            IElement guestTeamPanel = this.CreateTeamDisplay(guestTeam);

            var grid = new Grid
                {
                    Background = new SolidColorBrush(Colors.Black),
                    ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = GridLength.Auto }, 
                            new ColumnDefinition(),
                            new ColumnDefinition { Width = GridLength.Auto }
                        },
                    RowDefinitions =
                        {
                            new RowDefinition { Height = GridLength.Auto },
                            new RowDefinition { Height = GridLength.Auto } 
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
                    Height = 350,
                    VerticalAlignment = VerticalAlignment.Top,
                    BorderBrush = new SolidColorBrush(Colors.White),
                    BorderThickness = new Thickness(5),
                    Child = grid,
                };

            var homeButton = new Button
                {
                    Content =
                        new Border
                            {
                                Background = new SolidColorBrush(Colors.Gray),
                                Child = new TextBlock(this.basicFont) { Text = "Home Score" },
                            },
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(10)
                };

            homeButton.Click += (sender, args) => homeTeam.IncrementScore(1);
            grid.Children.Add(homeButton);
            Grid.SetRow(homeButton, 1);

            var guestButton = new Button
                {
                    Content =
                        new Border
                            {
                                Background = new SolidColorBrush(Colors.Gray),
                                Child = new TextBlock(this.basicFont) { Text = "Guest Score" },
                            },
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(10)
                };

            guestButton.Click += (sender, args) => guestTeam.IncrementScore(1);
            grid.Children.Add(guestButton);
            Grid.SetRow(guestButton, 1);
            Grid.SetColumn(guestButton, 2);

            this.rootElement.Content = border;
        }

        private IElement CreateTeamDisplay(Team team)
        {
            var teamNameTextBlock = new TextBlock(this.largeLabel)
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(10)
                };

            var scoreTextBlock = new TextBlock(this.largeLed)
                {
                    Foreground = new SolidColorBrush(Colors.Green),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(10)
                };

            teamNameTextBlock.Bind(TextBlock.TextProperty, BindingFactory.CreateOneWay<Team, string>(o => o.Name));
            scoreTextBlock.Bind(TextBlock.TextProperty, BindingFactory.CreateOneWay<Team, int, string>(o => o.Score));

            return new StackPanel { Children = { teamNameTextBlock, scoreTextBlock }, DataContext = team };
        }
    }
}