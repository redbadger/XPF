namespace RedBadger.PocketMechanic.Phone
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    using Microsoft.Phone.Reactive;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;
    using RedBadger.Xpf.Presentation.Media.Imaging;

    using GridLength = RedBadger.Xpf.Presentation.GridLength;
    using HorizontalAlignment = RedBadger.Xpf.Presentation.HorizontalAlignment;
    using SolidColorBrush = RedBadger.Xpf.Presentation.Media.SolidColorBrush;
    using Stretch = RedBadger.Xpf.Presentation.Media.Stretch;
    using Thickness = RedBadger.Xpf.Presentation.Thickness;
    using VerticalAlignment = RedBadger.Xpf.Presentation.VerticalAlignment;

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
            var badger = this.Game.Content.Load<Texture2D>("badger");
            this.spriteBatchAdapter = new SpriteBatchAdapter(this.GraphicsDevice);
            var spriteFontAdapter = new SpriteFontAdapter(this.spriteFont);

            var grid = new Grid();
            var column1 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200) });

            var stackpanel = new StackPanel { Orientation = Orientation.Horizontal };

            /*
            var sb = new Storyboard();
            var doubleAnimation = new DoubleAnimation()
                {
                    Duration = new Duration(TimeSpan.FromSeconds(4)),
                    From = 10,
                    To = 200,
                    RepeatBehavior = RepeatBehavior.Forever
                };
            Storyboard.SetTarget(doubleAnimation, column1);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("MaxWidth"));
            sb.Children.Add(doubleAnimation);
            sb.Begin();
*/
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
                    Padding = new Thickness(10, 10), 
                    HorizontalAlignment = HorizontalAlignment.Right, 
                    VerticalAlignment = VerticalAlignment.Bottom
                };
            Grid.SetColumn(textBlock3, 0);
            Grid.SetRow(textBlock3, 1);
            grid.Children.Add(textBlock3);

            var image = new Image
                {
                    Source = new XnaImage(new Texture2DAdapter(badger)),
                    Stretch = Stretch.Fill,
                    Margin = new Thickness(10)
                };
            Grid.SetColumn(image, 1);
            Grid.SetRow(image, 1);
            grid.Children.Add(image);

            /*
            var textBlock5 = new TextBlock(spriteFontAdapter) { Text = "TextBlock 5!" };
            Grid.SetColumn(textBlock5, 0);
            Grid.SetRow(textBlock5, 0);
            grid.Children.Add(textBlock5);
*/
            var viewPort = new Rect(
                this.GraphicsDevice.Viewport.X, 
                this.GraphicsDevice.Viewport.Y, 
                this.GraphicsDevice.Viewport.Width, 
                this.GraphicsDevice.Viewport.Height);

            this.rootElement =
                new RootElement(
                    new Renderer(this.spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice)), viewPort)
                    {
                       Content = grid 
                    };
            var clock = new Clock();
            Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).ObserveOnDispatcher().Subscribe(
                l => this.textBlock2.Text = clock.Time);

            // GC.Collect();
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
                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }
    }
}