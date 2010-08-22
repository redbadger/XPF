namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class VirtualizingElementCollection : IList<IElement>, ITemplatedList<IElement>
    {
        private readonly IList<Memento> items = new List<Memento>();

        private readonly IElement owner;

        private readonly IList<IElement> realizedElements = new List<IElement>();

        public VirtualizingElementCollection(IElement owner)
        {
            this.owner = owner;
        }

        public int Count
        {
            get
            {
                return this.items.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IList<IElement> RealizedElements
        {
            get
            {
                return this.realizedElements;
            }
        }

        public IElement this[int index]
        {
            get
            {
                var memento = this.items[index];
                if (memento.IsReal)
                {
                    return memento.Element;
                }

                return memento.Create();
            }

            set
            {
                throw new NotSupportedException("Please use the overload that takes a template");
            }
        }

        public bool IsReal(int index)
        {
            return this.items[index].IsReal;
        }

        public IElement Realize(int index)
        {
            var memento = this.items[index];

            if (memento.IsReal)
            {
                return memento.Element;
            }

            var element = memento.Realize();
            this.realizedElements.Add(element);
            element.VisualParent = this.owner;
            this.owner.InvalidateMeasure();
            return element;
        }

        public void Virtualize(int index)
        {
            var memento = this.items[index];

            if (memento.IsReal)
            {
                var element = memento.Virtualize();
                element.VisualParent = null;
                this.realizedElements.Remove(element);
                this.owner.InvalidateMeasure();
            }
        }

        public void Add(IElement element)
        {
            throw new NotSupportedException("Please use the overload that takes a template");
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public bool Contains(IElement item)
        {
            return false;
        }

        public void CopyTo(IElement[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IElement item)
        {
            throw new NotSupportedException("Please use RemoveAt(int index)");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<IElement> GetEnumerator()
        {
            return this.items.Select(memento => memento.IsReal ? memento.Element : memento.Create()).GetEnumerator();
        }

        public int IndexOf(IElement item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, IElement item)
        {
            throw new NotSupportedException(
                "Please use Insert(int index, object item, Func<object, IElement> template)");
        }

        public void RemoveAt(int index)
        {
            this.items.RemoveAt(index);
        }

        public virtual void Add(object item, Func<object, IElement> template)
        {
            this.items.Add(new Memento(item, template));
        }

        public virtual void Insert(int index, object item, Func<object, IElement> template)
        {
            this.items.Insert(index, new Memento(item, template));
        }

        public virtual void Move(int oldIndex, int newIndex)
        {
            var memento = this.items[oldIndex];
            this.items.RemoveAt(oldIndex);
            this.items.Insert(newIndex, memento);
        }

        private class Memento
        {
            private readonly object item;

            private readonly Func<object, IElement> template;

            private IElement element;

            public Memento(object item, Func<object, IElement> template)
            {
                if (template == null)
                {
                    throw new InvalidOperationException("A Template for this Item has not been supplied");
                }

                this.item = item;
                this.template = template;
            }

            public IElement Element
            {
                get
                {
                    return this.element;
                }
            }

            public bool IsReal
            {
                get
                {
                    return this.element != null;
                }
            }

            public IElement Create()
            {
                return this.template(this.item);
            }

            public IElement Realize()
            {
                this.element = this.Create();
                this.element.DataContext = this.item;
                return this.element;
            }

            public IElement Virtualize()
            {
                var virtualized = this.element;
                this.element = null;
                return virtualized;
            }
        }
    }
}