namespace RedBadger.PocketMechanic.Phone
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Input;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;

    using SolidColorBrush = RedBadger.Xpf.Presentation.Media.SolidColorBrush;

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

            var canvas = new Canvas();

            var textBlock = new TextBlock(this.font) { Text = "Test", Background = new SolidColorBrush(Colors.Red) };
            Canvas.SetLeft(textBlock, 50);
            Canvas.SetTop(textBlock, 200);
            canvas.Children.Add(textBlock);

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager())
                {
                    Content = canvas 
                };
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