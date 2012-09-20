#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

namespace Xpf.Samples.S05DataBinding101.WithoutBindingFactory
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf;
    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Adapters.Xna.Input;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Controls.Primitives;
    using RedBadger.Xpf.Media;
    using RedBadger.Xpf.Media.Imaging;

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
            var renderer = new Renderer(spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice));
            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager());

            // Setup Layout
            var cardImage = new Image { Stretch = Stretch.None };

            var cardToggleButton = new ToggleButton
                {
                    Content = cardImage,
                    Margin = new Thickness(10)
                };

            var resetButton = new Button
                {
                    Content =
                        new Border
                            {
                                Background = new SolidColorBrush(Colors.LightGray), 
                                Child = new TextBlock(spriteFontAdapter)
                                    {
                                        Text = "Reset",
                                        Margin = new Thickness(10)
                                    }
                            }, 
                    Margin = new Thickness(10), 
                    HorizontalAlignment = HorizontalAlignment.Center
                };

            var stackPanel = new StackPanel
                {
                    Children =
                        {
                            cardToggleButton,
                            resetButton
                        }
                };

            this.rootElement.Content = stackPanel;

            // Setup Data Binding
            var faceDownImage = new TextureImage(new Texture2DAdapter(this.Game.Content.Load<Texture2D>("FaceDown")));
            var faceUpImage = new TextureImage(new Texture2DAdapter(this.Game.Content.Load<Texture2D>("FaceUp")));

            var card = new Card(faceDownImage, faceUpImage);

            cardImage.Bind(
                Image.SourceProperty,
                card.CardImage);

            cardToggleButton.Bind(
                ToggleButton.IsCheckedProperty,
                card.IsCardFaceUp,
                card.IsCardFaceUp);

            resetButton.Click += (sender, args) => card.Reset();
        }
    }
}
