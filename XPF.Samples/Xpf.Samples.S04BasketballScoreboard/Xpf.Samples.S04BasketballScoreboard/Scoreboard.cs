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

    public class Scoreboard : DrawableGameComponent
    {
        private Team guestTeam;

        private Team homeTeam;

        private SpriteFontAdapter largeLabel;

        private SpriteFontAdapter largeLed;

        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatchAdapter;

        public Scoreboard(BasketballGame game)
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
            this.homeTeam = new Team("HOME");
            this.guestTeam = new Team("GUEST");

            this.spriteBatchAdapter = new SpriteBatchAdapter(new SpriteBatch(this.GraphicsDevice));
            var primitivesService = new PrimitivesService(this.GraphicsDevice);
            var renderer = new Renderer(this.spriteBatchAdapter, primitivesService);

            var smallLabel = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("SmallLabel"));
            this.largeLabel = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("SmallLabel"));

            var smallLed = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("SmallLed"));
            this.largeLed = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("LargeLed"));

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager());

            Observable.FromEvent<EventArgs>(
                handler => this.Game.Window.OrientationChanged += handler, 
                handler => this.Game.Window.OrientationChanged -= handler).Subscribe(
                    _ => this.rootElement.Viewport = this.Game.GraphicsDevice.Viewport.ToRect());

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
                                        new TextBlock(this.largeLed)
                                            {
                                                Text = "12:04", 
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
                                            new TextBlock(smallLabel)
                                                {
                                                    Text = "PERIOD", 
                                                    Foreground = new SolidColorBrush(Colors.White), 
                                                    Padding = new Thickness(10)
                                                }, 
                                            new TextBlock(smallLed)
                                                {
                                                    Text = "4", 
                                                    Foreground = new SolidColorBrush(Colors.Yellow), 
                                                    Padding = new Thickness(10)
                                                }
                                        }
                                }
                        }
                };

            IElement homeTeamPanel = this.CreateTeamDisplay(this.homeTeam);
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
                    VerticalAlignment = VerticalAlignment.Top, 
                    BorderBrush = new SolidColorBrush(Colors.White), 
                    BorderThickness = new Thickness(5), 
                    Child = grid, 
                };

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
                    Text = "113", 
                    Foreground = new SolidColorBrush(Colors.Green), 
                    HorizontalAlignment = HorizontalAlignment.Center, 
                    Padding = new Thickness(10)
                };

            teamNameTextBlock.Bind(TextBlock.TextProperty, BindingFactory.CreateOneWay<Team, string>(o => o.Name));
            scoreTextBlock.Bind(TextBlock.TextProperty, BindingFactory.CreateOneWay<Team, int, string>(o => o.Score));

            var teamPanel = new StackPanel { Children = { teamNameTextBlock, scoreTextBlock }, DataContext = team };

            return teamPanel;
        }
    }
}