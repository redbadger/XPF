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

namespace RedBadger.Wpug
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class CenteredText : DrawableGameComponent
    {
        private Vector2 drawPosition;

        private SpriteBatch spriteBatch;

        private SpriteFont spriteFont;

        private string text;

        public CenteredText(Game game)
            : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            this.spriteBatch.DrawString(this.spriteFont, this.text, this.drawPosition, Color.Black);

            this.spriteBatch.End();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.spriteFont = this.Game.Content.Load<SpriteFont>("SpriteFont");

            this.text = "Windows Phone User Group";

            Viewport viewport = this.GraphicsDevice.Viewport;
            Vector2 measureString = this.spriteFont.MeasureString(this.text);

            this.drawPosition.X = (viewport.Width / 2f) - (measureString.X / 2f);
            this.drawPosition.Y = (viewport.Height / 2f) - (measureString.Y / 2f);
        }
    }
}
