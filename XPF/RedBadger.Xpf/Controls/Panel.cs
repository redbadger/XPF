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

namespace RedBadger.Xpf.Controls
{
    using System.Collections.Generic;
    using System.Linq;

    using RedBadger.Xpf.Graphics;
    using RedBadger.Xpf.Media;

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
