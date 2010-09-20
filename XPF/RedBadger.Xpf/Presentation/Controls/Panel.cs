namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Collections.Generic;
    using System.Linq;

    using RedBadger.Xpf.Presentation.Media;

    public abstract class Panel : UIElement
    {
        public static readonly ReactiveProperty<Brush> BackgroundProperty =
            ReactiveProperty<Brush>.Register("Background", typeof(Panel));

        private IList<IElement> children;

        public Brush Background
        {
            get
            {
                return this.GetValue(BackgroundProperty);
            }

            set
            {
                this.SetValue(BackgroundProperty, value);
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

        protected virtual IList<IElement> CreateChildrenCollection()
        {
            return new ElementCollection(this);
        }

        protected override void OnRender(IDrawingContext drawingContext)
        {
            if (this.Background != null)
            {
                drawingContext.DrawRectangle(new Rect(0, 0, this.ActualWidth, this.ActualHeight), this.Background);
            }
        }

        private void EnsureChildrenCollection()
        {
            if (this.children == null)
            {
                this.children = this.CreateChildrenCollection();
            }
        }
    }
}