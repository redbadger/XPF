/*namespace RedBadger.PocketMechanic.Phone
{
    using System.ComponentModel;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Input;
    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;

    public class XpfTest : DrawableGameComponent
    {
        private StackPanel commentaryBox;

        private SpriteFontAdapter font;

        private MyBindingObject myBindingObject;

        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatchAdapter;

        private SpriteFont spriteFont;

        private TextBlock textBlock2;

        public XpfTest(Game game)
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
            this.font = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("SpriteFont"));
            this.spriteBatchAdapter = new SpriteBatchAdapter(this.GraphicsDevice);

            var renderer = new Renderer(this.spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice));

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager())
                {
                    Content =
                        new Grid
                            {
                                HorizontalAlignment = HorizontalAlignment.Center, 
                                ColumnDefinitions =
                                    {
                                        new ColumnDefinition { Width = new GridLength(25, GridUnitType.Pixel), }, 
                                        new ColumnDefinition { Width = new GridLength(50, GridUnitType.Pixel), }, 
                                        new ColumnDefinition
                                            {
                                               Width = new GridLength(1024 - (50 * 2) - (25 * 2), GridUnitType.Pixel), 
                                            }, 
                                        new ColumnDefinition { Width = new GridLength(50, GridUnitType.Pixel), }, 
                                        new ColumnDefinition { Width = new GridLength(25, GridUnitType.Pixel), }, 
                                    }, 
                                RowDefinitions =
                                    {
                                        new RowDefinition { Height = new GridLength(25, GridUnitType.Pixel), }, 
                                        new RowDefinition { Height = new GridLength(760 - 25, GridUnitType.Pixel), }
                                    }
                            }
                };

            /*Image team1Bubble = new Image() { Source = new XnaImage(new Texture2DAdapter(Content.Load<Texture2D>("bulb"))) }; // todo need a way to tint these.
            Image team2Bubble = new Image() { Source = new XnaImage(new Texture2DAdapter(Content.Load<Texture2D>("bulb"))) }; // todo image brush would be useful, could make this the background for the text.♥1♥
            var team1 = new TextBlock(this.font) { Foreground = new SolidColorBrush(Colors.Red), Text = "Red" };
            var team2 = new TextBlock(this.font) { Foreground = new SolidColorBrush(Colors.Blue), Text = "Blue" };

            /*Grid.SetColumn(team1, 1); Grid.SetColumn(team1Bubble, 1);
            Grid.SetColumn(team2Bubble, 3); Grid.SetColumn(team2, 3);♥1♥
            (this.rootElement.Content as Grid).Children.Add(team1);

            /*(this.rootElement.Content as Grid).Children.Add(team1Bubble);// note adding things to a grid cell does not have any impact on the layout order (text goes first on both sides, dispite the image being added first for the right hand UI side here)
            (this.rootElement.Content as Grid).Children.Add(team2Bubble);♥1♥
            (this.rootElement.Content as Grid).Children.Add(team2);

            this.commentaryBox = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center, 
                    VerticalAlignment = VerticalAlignment.Bottom, 
                    Orientation = Orientation.Vertical, 
                };
            Grid.SetColumn(this.commentaryBox, 2);
            Grid.SetRow(this.commentaryBox, 1);
            (this.rootElement.Content as Grid).Children.Add(this.commentaryBox);

            var resetButton = new Button
                {
                    Content =
                        /*new Border()
                                                       {
                                                           BorderBrush = new SolidColorBrush(Colors.Black),
                                                           BorderThickness = new Thickness(2),
                                                           Child = ♥1♥
                        new TextBlock(this.font)
                            {
                                Text = "Reset", 
                                Background = new SolidColorBrush(Colors.Green), 
                                Foreground = new SolidColorBrush(Colors.Blue), 
                                Padding = new Thickness(10), 
                                // bug add padding here and the background draws in the wrong location.
                            }, 
                    // },
                    // Padding = new Thickness(10), // bug Add padding here and the text block's background collapses.
                    HorizontalAlignment = HorizontalAlignment.Center, 
                    VerticalAlignment = VerticalAlignment.Center, 
                };

            // resetButton.Click += (sender, args) => ((Ball)this.Components.First(component => component is Ball)).Reset();
            Grid.SetColumn(resetButton, 2);
            Grid.SetRow(resetButton, 0);
            (this.rootElement.Content as Grid).Children.Add(resetButton);
        }

        public class BorderBrushConverter : INotifyPropertyChanged
        {
            private SolidColorBrush brush;

            private bool isPressed;

            public event PropertyChangedEventHandler PropertyChanged;

            public SolidColorBrush Brush
            {
                get
                {
                    return this.brush;
                }

                set
                {
                    this.brush = value;
                    this.InvokePropertyChanged(new PropertyChangedEventArgs<TProperty, TOwner>("Brush"));
                }
            }

            public bool IsPressed
            {
                get
                {
                    return this.isPressed;
                }

                set
                {
                    this.isPressed = value;
                    this.Brush = this.isPressed ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green);
                }
            }

            public void InvokePropertyChanged(PropertyChangedEventArgs<TProperty, TOwner> e)
            {
                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        public class MyBindingObject : INotifyPropertyChanged
        {
            private string myWidth;

            public event PropertyChangedEventHandler PropertyChanged;

            public string MyWidth
            {
                get
                {
                    return this.myWidth;
                }

                set
                {
                    this.myWidth = value;
                    this.InvokePropertyChanged(new PropertyChangedEventArgs<TProperty, TOwner>("MyWidth"));
                }
            }

            public void InvokePropertyChanged(PropertyChangedEventArgs<TProperty, TOwner> e)
            {
                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }
    }
}*/