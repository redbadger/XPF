namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using RedBadger.Xpf.Presentation.Controls.Primitives;
    using RedBadger.Xpf.Presentation.Media;

    using UIElement = RedBadger.Xpf.Presentation.UIElement;

    public abstract class Panel : UIElement, IPanel
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

        public void AddChild(object newItem, Func<object, IElement> containerGenerator)
        {
            this.children.Add(NewContainer(newItem, containerGenerator));
        }

        public void ClearChildren()
        {
            this.children.Clear();
        }

        public void InsertChildAt(int index, object newItem, Func<object, IElement> containerGenerator)
        {
            this.children.Insert(index, NewContainer(newItem, containerGenerator));
        }

        public void MoveChild(int oldIndex, int newIndex)
        {
            var element = this.children[oldIndex];
            this.children.RemoveAt(oldIndex);
            this.children.Insert(newIndex, element);
        }

        public void RemoveChildAt(int index)
        {
            this.children.RemoveAt(index);
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

        private static IElement NewContainer(object item, Func<object, IElement> containerGenerator)
        {
            if (containerGenerator == null)
            {
                throw new InvalidOperationException("A Template for this Item has not been supplied");
            }

            var element = containerGenerator(item);
            element.DataContext = item;
            return element;
        }
    }
}