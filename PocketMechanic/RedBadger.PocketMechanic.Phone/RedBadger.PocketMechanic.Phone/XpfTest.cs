namespace RedBadger.PocketMechanic.Phone
{
    using System;
    using System.ComponentModel;
    using System.Windows.Data;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Controls;

    public class XpfTest : DrawableGameComponent
    {
        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatchAdapter;

        private SpriteFont spriteFont;

        private ViewModel viewModel;

        public XpfTest(Game game)
            : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatchAdapter.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            this.rootElement.Draw(this.spriteBatchAdapter);
            this.spriteBatchAdapter.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.viewModel.DisplayText = DateTime.Now.ToString();
            this.rootElement.Update();
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            this.spriteFont = this.Game.Content.Load<SpriteFont>("SpriteFont");
            this.spriteBatchAdapter = new SpriteBatchAdapter(this.GraphicsDevice);

            this.viewModel = new ViewModel();

            var textBlock = new TextBlock(new SpriteFontAdapter(this.spriteFont)) { Text = "Hello Phone!" };
            this.rootElement =
                new RootElement(
                    new Rect(
                        this.GraphicsDevice.Viewport.X, 
                        this.GraphicsDevice.Viewport.Y, 
                        this.GraphicsDevice.Viewport.Width, 
                        this.GraphicsDevice.Viewport.Height))
                    {
                       Content = textBlock 
                    };

            var binding = new Binding("DisplayText");
            binding.Source = this.viewModel;
            BindingOperations.SetBinding(textBlock, TextBlock.TextProperty, binding);
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        private string displayText;

        public event PropertyChangedEventHandler PropertyChanged;

        public string DisplayText
        {
            get
            {
                return this.displayText;
            }

            set
            {
                if (this.displayText != value)
                {
                    this.displayText = value;
                    this.OnPropertyChanged("DisplayText");
                }
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}