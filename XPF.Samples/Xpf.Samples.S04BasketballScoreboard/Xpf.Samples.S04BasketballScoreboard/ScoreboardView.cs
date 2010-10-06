namespace Xpf.Samples.S04BasketballScoreboard
{
    using System;

    using Microsoft.Phone.Reactive;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf;
    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Adapters.Xna.Input;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Data;
    using RedBadger.Xpf.Media;

    using Xpf.Samples.S04BasketballScoreboard.Domain;

    public class ScoreboardView : DrawableGameComponent
    {
        private readonly Clock clock;

        private readonly Team guestTeam;

        private readonly Team homeTeam;

        private SpriteFontAdapter led;

        private RootElement rootElement;

        private SpriteFontAdapter lcd;

        public ScoreboardView(BasketballGame game, Team homeTeam, Team guestTeam, Clock clock)
            : base(game)
        {
            // this.Visible = false;
            this.homeTeam = homeTeam;
            this.guestTeam = guestTeam;
            this.clock = clock;
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

            this.lcd = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("Lcd"));
            this.led = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("Led"));

            Observable.FromEvent<EventArgs>(
                handler => this.Game.Window.OrientationChanged += handler, 
                handler => this.Game.Window.OrientationChanged -= handler).Subscribe(
                    _ => this.rootElement.Viewport = this.Game.GraphicsDevice.Viewport.ToRect());

            var timeTextBlock = new TextBlock(this.led)
                {
                   Foreground = new SolidColorBrush(Colors.Red), HorizontalAlignment = HorizontalAlignment.Center 
                };

            timeTextBlock.Bind(TextBlock.TextProperty, this.clock.TimeDisplay);

            var periodTextBlock = new TextBlock(this.led)
                {
                   Foreground = new SolidColorBrush(Colors.Yellow), Padding = new Thickness(10), VerticalAlignment = VerticalAlignment.Center
                };
            periodTextBlock.Bind(
                TextBlock.TextProperty, BindingFactory.CreateOneWay<Clock, int, string>(this.clock, c => c.Period));

            IElement homeTeamPanel = this.CreateTeamDisplay(this.homeTeam);

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
                                    Width = 280, 
                                    Child = timeTextBlock
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
                                            periodTextBlock
                                        }
                                }
                        }
                };

            IElement guestTeamPanel = this.CreateTeamDisplay(this.guestTeam);

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
                    Height = 280, 
                    VerticalAlignment = VerticalAlignment.Top,
                    BorderBrush = new SolidColorBrush(Colors.LightGray), 
                    BorderThickness = new Thickness(5), 
                    Child = grid, 
                };

            this.rootElement.Content = border;
        }

        private IElement CreateTeamDisplay(Team team)
        {
            var teamNameTextBlock = new TextBlock(this.lcd)
                {
                    Foreground = new SolidColorBrush(Colors.LightGray), 
                    HorizontalAlignment = HorizontalAlignment.Center, 
                    Padding = new Thickness(25)
                };

            var scoreTextBlock = new TextBlock(this.led)
                {
                    Foreground = new SolidColorBrush(Colors.Green), 
                    HorizontalAlignment = HorizontalAlignment.Center
                };

            teamNameTextBlock.Bind(TextBlock.TextProperty, BindingFactory.CreateOneWay<Team, string>(o => o.Name));
            scoreTextBlock.Bind(TextBlock.TextProperty, BindingFactory.CreateOneWay<Team, int, string>(o => o.Score));

            return new StackPanel { Children = { teamNameTextBlock, scoreTextBlock }, DataContext = team };
        }
    }
}