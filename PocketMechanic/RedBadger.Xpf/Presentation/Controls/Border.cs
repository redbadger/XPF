namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Presentation.Media;

#if WINDOWS_PHONE
    using UIElement = RedBadger.Xpf.Presentation.UIElement;
#endif

    public class Border : UIElement
    {
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background", typeof(Brush), typeof(Border), new PropertyMetadata(null));

        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            "BorderBrush", typeof(Brush), typeof(Border), new PropertyMetadata(null));

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register(
                "BorderThickness", typeof(Thickness), typeof(Border), new PropertyMetadata(null));

        public static readonly DependencyProperty ChildProperty = DependencyProperty.Register(
            "Child", typeof(UIElement), typeof(Border), new PropertyMetadata(null));

        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            "Padding", typeof(Thickness), typeof(Border), new PropertyMetadata(null));

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

        public Brush BorderBrush
        {
            get
            {
                return (Brush)this.GetValue(BorderBrushProperty);
            }

            set
            {
                this.SetValue(BorderBrushProperty, value);
            }
        }

        public Thickness BorderThickness
        {
            get
            {
                return (Thickness)this.GetValue(BorderThicknessProperty);
            }

            set
            {
                this.SetValue(BorderThicknessProperty, value);
            }
        }

        public UIElement Child
        {
            get
            {
                return (UIElement)this.GetValue(ChildProperty);
            }

            set
            {
                this.SetValue(ChildProperty, value);
            }
        }

        public Thickness Padding
        {
            get
            {
                return (Thickness)this.GetValue(PaddingProperty);
            }

            set
            {
                this.SetValue(PaddingProperty, value);
            }
        }

        public override void Render(ISpriteBatch spriteBatch)
        {
        }
    }
}