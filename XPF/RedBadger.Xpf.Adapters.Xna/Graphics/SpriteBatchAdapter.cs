namespace RedBadger.Xpf.Adapters.Xna.Graphics
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Extensions;
    using RedBadger.Xpf.Graphics;

    using Color = RedBadger.Xpf.Media.Color;
    using Point = RedBadger.Xpf.Point;

    /// <summary>
    ///     Adapts an XNA <see cref = "SpriteBatch">SpriteBatch</see> to an XPF <see cref = "ISpriteBatch">ISpriteBatch</see>.
    /// </summary>
    public class SpriteBatchAdapter : ISpriteBatch
    {
        private static readonly RasterizerState scissorTestingRasterizerState = new RasterizerState
            {
               ScissorTestEnable = true, CullMode = CullMode.None 
            };

        private readonly Stack<IDrawingContext> clipRegions = new Stack<IDrawingContext>();

        private readonly SpriteBatch spriteBatch;

        private Rect? currentClippingRect;

        private bool isBatchOpen;

        private IDrawingContext parentContext;

        /// <summary>
        ///     Initializes a new instance of a <see cref = "SpriteBatchAdapter">SpriteBatchAdapter</see>.
        /// </summary>
        /// <param name = "spriteBatch">The XNA <see cref = "SpriteBatch">SpriteBatch</see> to adapt.</param>
        public SpriteBatchAdapter(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        public void Begin(IDrawingContext drawingContext)
        {
            while (this.clipRegions.Count > 0 && !drawingContext.Element.IsDescendantOf(this.clipRegions.Peek().Element))
            {
                this.clipRegions.Pop();
                this.parentContext = this.clipRegions.Count == 0 ? null : this.clipRegions.Peek();
            }

            if (!drawingContext.ClippingRect.IsEmpty)
            {
                this.parentContext = this.clipRegions.Count == 0 ? null : this.clipRegions.Peek();
                this.clipRegions.Push(drawingContext);
            }

            Rect clippingRect = Rect.Empty;
            if (this.clipRegions.Count > 0)
            {
                IDrawingContext context = this.clipRegions.Peek();
                clippingRect = context.ClippingRect;
                clippingRect.Displace(context.AbsoluteOffset);
                if (this.parentContext != null)
                {
                    var parentClip = this.parentContext.ClippingRect;
                    parentClip.Displace(this.parentContext.AbsoluteOffset);
                    clippingRect.Intersect(parentClip);
                }
            }

            if (this.currentClippingRect != clippingRect)
            {
                this.currentClippingRect = clippingRect;

                if (this.isBatchOpen)
                {
                    this.End();
                }

                if (clippingRect == Rect.Empty)
                {
                    this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                }
                else
                {
                    this.spriteBatch.GraphicsDevice.ScissorRectangle = clippingRect.ToRectangle();

                    this.spriteBatch.Begin(
                        SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, scissorTestingRasterizerState);
                }

                this.isBatchOpen = true;
            }
        }

        public void Draw(ITexture2D texture2D, Rect rect, Color color)
        {
            var texture2DAdapter = texture2D as Texture2DAdapter;
            if (texture2DAdapter != null && texture2DAdapter.Value != null)
            {
                var rectangle = new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
                this.spriteBatch.Draw(
                    texture2DAdapter.Value, 
                    rectangle, 
                    new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A));
            }
        }

        public void DrawString(ISpriteFont spriteFont, string text, Point position, Color color)
        {
            var spriteFontAdapter = spriteFont as SpriteFontAdapter;
            if (spriteFontAdapter != null && spriteFontAdapter.Value != null)
            {
                this.spriteBatch.DrawString(
                    spriteFontAdapter.Value, 
                    text ?? string.Empty, 
                    new Vector2((float)position.X, (float)position.Y), 
                    new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A));
            }
        }

        public void End()
        {
            this.spriteBatch.End();
            this.isBatchOpen = false;
            this.currentClippingRect = null;
        }
    }
}