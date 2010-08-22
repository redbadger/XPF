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

        private IList<IElement> children;

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
                this.EnsureChildrenCollection();
                return this.children;
            }

            protected set
            {
                this.children = value;
            }
        }

        public override IEnumerable<IElement> GetVisualChildren()
        {
            this.EnsureChildrenCollection();
            return this.children.AsEnumerable();
        }

        protected virtual void CreateChildrenCollection()
        {
            this.children = new ElementCollection(this);
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

        private void EnsureChildrenCollection()
        {
            if (this.children == null)
            {
                this.CreateChildrenCollection();
            }
        }
    }
}