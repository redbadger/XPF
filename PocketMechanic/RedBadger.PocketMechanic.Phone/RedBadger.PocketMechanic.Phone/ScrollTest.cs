namespace RedBadger.PocketMechanic.Phone
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    using Microsoft.Phone.Reactive;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Input;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;
    using RedBadger.Xpf.Presentation.Media.Imaging;

    using SolidColorBrush = RedBadger.Xpf.Presentation.Media.SolidColorBrush;

    public class ScrollTest : DrawableGameComponent
    {
        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatchAdapter;

        private SpriteFont spriteFont;

        public ScrollTest(Game game)
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
            this.spriteBatchAdapter = new SpriteBatchAdapter(this.GraphicsDevice);
            var spriteFontAdapter = new SpriteFontAdapter(this.spriteFont);
            var items = new ObservableCollection<string>();

            var itemsControl = new ItemsControl
                {
                    ItemTemplate = () =>
                        {
                            var textBlock = new TextBlock(spriteFontAdapter);
                            textBlock.SetBinding(TextBlock.TextProperty, new Binding());
                            return textBlock;
                        }
                };
            itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = items });

            var scrollViewer = new ScrollViewer { Content = itemsControl, CanHorizontallyScroll = false };

            var viewPort = new Rect(
                this.GraphicsDevice.Viewport.X,
                this.GraphicsDevice.Viewport.Y,
                this.GraphicsDevice.Viewport.Width,
                this.GraphicsDevice.Viewport.Height);

            var renderer = new Renderer(this.spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice));
            this.rootElement = new RootElement(viewPort, renderer, new InputManager()) { Content = scrollViewer };

            Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).ObserveOnDispatcher().Subscribe(
                l => items.Add(DateTime.Now.ToString()));
        }
    }
}