namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Media;

#if WINDOWS_PHONE
    using UIElement = RedBadger.Xpf.Presentation.UIElement;
#endif
    
    public class Panel : UIElement
    {
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background", typeof(Brush), typeof(Panel), new PropertyMetadata(null));

        private readonly UIElementCollection children;

        public Panel()
        {
            this.children = new UIElementCollection(this);
        }

        public Brush Background
        {
            get
            {
                return (Brush)this.GetValue(BackgroundProperty);
            }

            set
            {
                this.SetValue(BackgroundProperty, value);
            }
        }

        public UIElementCollection Children
        {
            get
            {
                return this.children;
            }
        }

        public override void Render(ISpriteBatch spriteBatch)
        {
            this.RenderBackground(spriteBatch);
        }

        /// <summary>
        ///   TODO: this is not fully implemented (Texture and Rectangle)
        /// </summary>
        /// <param name = "spriteBatch"></param>
        private void RenderBackground(ISpriteBatch spriteBatch)
        {
            var brush = this.Background as SolidColorBrush;

            // need one pixel white texture here
            spriteBatch.Draw(
                (ITexture2D)null, 
                new Rectangle(0, 0, (int)this.ActualWidth, (int)this.ActualHeight),
                brush != null ? brush.Color : Color.White);
        }
    }
}