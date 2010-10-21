namespace RedBadger.PocketMechanic.Phone
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Adapters.Xna.Input;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Controls.Primitives;
    using RedBadger.Xpf.Data;
    using RedBadger.Xpf.Media;

    public class MyComponent : DrawableGameComponent
    {
        private RootElement rootElement;

        public MyComponent(Game game)
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
            var spriteBatchAdapter = new SpriteBatchAdapter(new SpriteBatch(this.GraphicsDevice));
            var spriteFontAdapter = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("SpriteFont"));
            var primitivesService = new PrimitivesService(this.GraphicsDevice);
            var renderer = new Renderer(spriteBatchAdapter, primitivesService);

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager());

            var bindingClass = new BindingClass();

            var border = new Border
                {
                    Background = new SolidColorBrush(Colors.LightGray), 
                    Child = new TextBlock(spriteFontAdapter) { Text = "Click" }
                };
            border.Bind(Border.BackgroundProperty, BindingFactory.CreateOneWay<BindingClass, Brush>(bindingClass, b => b.BackgroundColor));

            var toggleButton = new ToggleButton { Content = border };
            toggleButton.Bind(ToggleButton.IsCheckedProperty, BindingFactory.CreateTwoWay(bindingClass, b => b.IsChecked));

            this.rootElement.Content = toggleButton;
        }
    }

    public class BindingClass : INotifyPropertyChanged
    {
        private SolidColorBrush backgroundColor;

        private bool? isChecked = false;

        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        public SolidColorBrush BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }

            set
            {
                if (this.backgroundColor != value)
                {
                    this.backgroundColor = value;
                    this.OnPropertyChanged("BackgroundColor");
                }
            }
        }

        public bool? IsChecked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    this.OnPropertyChanged("IsChecked");
                    this.OnIsCheckedChanged(value);
                }
            }
        }

        private void OnIsCheckedChanged(bool? value)
        {
            if (value.HasValue)
            {
                this.BackgroundColor = (bool)value ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.LightGray);
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            EventHandler<PropertyChangedEventArgs> handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}