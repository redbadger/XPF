namespace RedBadger.PocketMechanic.Phone
{
    using System;

    using Microsoft.Phone.Reactive;
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
            var bindingClass2 = new BindingClass2();

            var border = new Border
                {
                    Background = new SolidColorBrush(Colors.LightGray), 
                    Child = new TextBlock(spriteFontAdapter) { Text = "Click" }
                };

            var toggleButton = new ToggleButton { Content = border };

            border.Bind(Border.BackgroundProperty, bindingClass2.BackgroundColor);

            toggleButton.Bind(ToggleButton.IsCheckedProperty, bindingClass2.IsChecked, bindingClass2.IsChecked);

            this.rootElement.Content = toggleButton;
        }
    }

    public class BindingClass2
    {
        private readonly Subject<Brush> backgroundColor = new Subject<Brush>();

        private Subject<bool?> isChecked = new Subject<bool?>();

        public BindingClass2()
        {
            this.isChecked.Subscribe(this.OnNextIsChecked);
        }

        public IObservable<Brush> BackgroundColor
        {
            get
            {
                return this.backgroundColor.AsObservable();
            }
        }

        public Subject<bool?> IsChecked
        {
            get
            {
                return this.isChecked;
            }
        }

        private void OnNextIsChecked(bool? value)
        {
            if (value.HasValue)
            {
                this.backgroundColor.OnNext(
                    (bool)value ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.LightGray));
            }
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

        public void OnPropertyChanged(string propertyName)
        {
            EventHandler<PropertyChangedEventArgs> handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnIsCheckedChanged(bool? value)
        {
            if (value.HasValue)
            {
                this.BackgroundColor = (bool)value
                                           ? new SolidColorBrush(Colors.Red)
                                           : new SolidColorBrush(Colors.LightGray);
            }
        }
    }
}