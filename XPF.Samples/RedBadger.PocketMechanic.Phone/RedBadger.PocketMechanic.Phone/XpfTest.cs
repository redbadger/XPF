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

namespace RedBadger.PocketMechanic.Phone
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Adapters.Xna.Input;
    using RedBadger.Xpf.Controls;

    public class XpfTest : DrawableGameComponent
    {
        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatchAdapter;

        private SpriteFontAdapter spriteFontAdapter;

        public XpfTest(Game game)
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
            this.spriteFontAdapter = new SpriteFontAdapter(this.Game.Content.Load<SpriteFont>("SpriteFont"));
            this.spriteBatchAdapter = new SpriteBatchAdapter(new SpriteBatch(this.GraphicsDevice));

            var renderer = new Renderer(this.spriteBatchAdapter, new PrimitivesService(this.GraphicsDevice));

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager());

            var itemsControl = new ItemsControl();

            // this.rootElement.Content = itemsControl;
        }
    }
}
