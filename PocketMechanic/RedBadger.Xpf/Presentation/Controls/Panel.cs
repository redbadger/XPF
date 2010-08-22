namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using RedBadger.Xpf.Presentation.Media;

    using UIElement = RedBadger.Xpf.Presentation.UIElement;

    public abstract class Panel : UIElement
    {
        public static readonly XpfDependencyProperty BackgroundProperty = XpfDependencyProperty.Register(
            "Background", typeof(Brush), typeof(Panel), new PropertyMetadata(null));

        private readonly ElementCollection children;

        protected Panel()
        {
            this.children = new ElementCollection(this);
        }

        public Brush Background
        {
            get
            {
                return (Brush)this.GetValue(BackgroundProperty.Value);
            }

            set
            {
                this.SetValue(BackgroundProperty.Value, value);
            }
        }

        public IList<IElement> Children
        {
            get
            {
                return this.children;
            }
        }

        public override IEnumerable<IElement> GetVisualChildren()
        {
            return this.children.AsEnumerable();
        }

        protected override void OnRender(IDrawingContext drawingContext)
        {
            if (this.Background != null)
            {
                drawingContext.DrawRectangle(
                    new Rect(
                        this.Margin.Left, 
                        this.Margin.Top, 
                        this.ActualWidth - (this.Margin.Left + this.Margin.Right), 
                        this.ActualHeight - (this.Margin.Top + this.Margin.Bottom)), 
                    this.Background);
            }
        }
    }
}