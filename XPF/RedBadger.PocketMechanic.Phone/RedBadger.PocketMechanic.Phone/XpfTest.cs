namespace RedBadger.PocketMechanic.Phone
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    using Microsoft.Phone.Reactive;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Input;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Controls.Primitives;
    using RedBadger.Xpf.Presentation.Media;
    using RedBadger.Xpf.Presentation.Media.Imaging;

    using SolidColorBrush = RedBadger.Xpf.Presentation.Media.SolidColorBrush;

    public class XpfTest : DrawableGameComponent
    {
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
            this.spriteFont = this.Game.Content.Load<SpriteFont>("SpriteFont");
            var badger2 = new XnaImage(new Texture2DAdapter(this.Game.Content.Load<Texture2D>("badger2")));
            var badger3 = new XnaImage(new Texture2DAdapter(this.Game.Content.Load<Texture2D>("badger3")));
            this.spriteBatchAdapter = new SpriteBatchAdapter(this.GraphicsDevice);
            var spriteFontAdapter = new SpriteFontAdapter(this.spriteFont);

            var grid = new Grid();
            var column1 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200) });

            var stackpanel = new StackPanel { Orientation = Orientation.Horizontal };

            var textBlock1 = new TextBlock(spriteFontAdapter);
            var border = new Border
                {
                    Child = textBlock1, 
                    BorderBrush = new SolidColorBrush(Colors.Red), 
                    BorderThickness = new Thickness(20), 
                    Background = new SolidColorBrush(Colors.Cyan)
                };

            Grid.SetColumn(border, 0);
            grid.Children.Add(border);

            var sb = new Storyboard();
            var doubleAnimation = new DoubleAnimation
                {
                    Duration = new Duration(TimeSpan.FromSeconds(4)), 
                    From = 30, 
                    To = 200, 
                    RepeatBehavior = RepeatBehavior.Forever, 
                    AutoReverse = true
                };
            Storyboard.SetTarget(doubleAnimation, textBlock1);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Width"));
            sb.Children.Add(doubleAnimation);
            sb.Begin();

            // stackpanel.Children.Add(border);
            this.myBindingObject = new MyBindingObject();
            textBlock1.SetBinding(TextBlock.TextProperty, new Binding("MyWidth") { Source = this.myBindingObject });

            this.textBlock2 = new TextBlock(spriteFontAdapter) { Text = "Textblock 2" };
            this.textBlock2.SetBinding(
                TextBlock.TextProperty, 
                new Binding("MyWidth") { Source = this.myBindingObject, Mode = BindingMode.TwoWay });

            // this.myBindingObject.MyWidth = "100";
            border = new Border
                {
                    Child = this.textBlock2, 
                    BorderBrush = new SolidColorBrush(Colors.Red), 
                    BorderThickness = new Thickness(20), 
                    Background = new SolidColorBrush(Colors.Cyan)
                };
            Grid.SetColumn(border, 1);
            grid.Children.Add(border);

            // stackpanel.Children.Add(border);
            var textBlock3 = new TextBlock(spriteFontAdapter)
                {
                    Text = "TextBlock 3", 
                    Background = new SolidColorBrush(Colors.Blue), 
                    Padding = new Thickness(10), 
                    HorizontalAlignment = HorizontalAlignment.Right, 
                    VerticalAlignment = VerticalAlignment.Bottom
                };
            Grid.SetColumn(textBlock3, 0);
            Grid.SetRow(textBlock3, 1);
            grid.Children.Add(textBlock3);

            var image = new Image
                {
                    Source = badger2, 
                    Stretch = Stretch.Fill, 
                    StretchDirection = StretchDirection.DownOnly, 
                };

            var imageBorder = new Border { Child = image, BorderThickness = new Thickness(10) };
            var button = new Button { Content = imageBorder };
            var borderBrushConverter = new BorderBrushConverter();
            imageBorder.SetBinding(Border.BorderBrushProperty, new Binding("Brush") { Source = borderBrushConverter });
            button.SetBinding(
                ButtonBase.IsPressedProperty,
                new Binding("IsPressed") { Source = borderBrushConverter, Mode = BindingMode.TwoWay });
            button.Click += (sender, args) => image.Source = image.Source == badger2 ? badger3 : badger2;
            Grid.SetColumn(button, 1);
            Grid.SetRow(button, 1);
            grid.Children.Add(button);

            /*
            var textBlock5 = new TextBlock(spriteFontAdapter) { Text = "TextBlock 5!" };
            Grid.SetColumn(textBlock5, 0);
            Grid.SetRow(textBlock5, 0);
            grid.Children.Add(textBlock5);
             * button
*/
            var viewPort = new Rect(
                this.GraphicsDevice.Viewport.X, 
                this.GraphicsDevice.Viewport.Y, 
                this.GraphicsDevice.Viewport.Width, 
                this.GraphicsDevice.Viewport.Height);

            this.rootElement = new RootElement(
                viewPort, 
                new Renderer(this.spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice)), 
                new InputManager()) {
                                       Content = grid 
                                    };
            var clock = new Clock();
            Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).ObserveOnDispatcher().Subscribe(
                l => this.textBlock2.Text = clock.Time);

            // GC.Collect();
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
                    this.InvokePropertyChanged(new PropertyChangedEventArgs("Brush"));
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

            public void InvokePropertyChanged(PropertyChangedEventArgs e)
            {
                var handler = this.PropertyChanged;
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
                    this.InvokePropertyChanged(new PropertyChangedEventArgs("MyWidth"));
                }
            }

            public void InvokePropertyChanged(PropertyChangedEventArgs e)
            {
                var handler = this.PropertyChanged;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }
    }
}