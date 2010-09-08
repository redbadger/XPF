namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ElementCollection : IList<IElement>, ITemplatedList<IElement>
    {
        private readonly List<IElement> elements = new List<IElement>();

        private readonly IElement owner;

        public ElementCollection(IElement owner)
        {
            this.owner = owner;
        }

        public int Count
        {
            get
            {
                return this.elements.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IElement this[int index]
        {
            get
            {
                return this.elements[index];
            }

            set
            {
                IElement oldItem = this.elements[index];
                IElement newItem = value;

                this.elements[index] = newItem;
                this.owner.InvalidateMeasure();
                this.SetParents(oldItem, newItem);
            }
        }

        public void Add(IElement item)
        {
            this.elements.Add(item);
            this.owner.InvalidateMeasure();
            this.SetParents(null, item);
        }

        public void Clear()
        {
            this.elements.Clear();
            this.owner.InvalidateMeasure();
        }

        public bool Contains(IElement item)
        {
            return this.elements.Contains(item);
        }

        public void CopyTo(IElement[] array, int arrayIndex)
        {
            this.elements.CopyTo(array, arrayIndex);
        }

        public bool Remove(IElement item)
        {
            bool wasRemoved = this.elements.Remove(item);
            if (wasRemoved)
            {
                this.owner.InvalidateMeasure();
                this.SetParents(item, null);
            }

            return wasRemoved;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<IElement> GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }

        public int IndexOf(IElement item)
        {
            return this.elements.IndexOf(item);
        }

        public void Insert(int index, IElement item)
        {
            this.elements.Insert(index, item);
            this.owner.InvalidateMeasure();
            this.SetParents(null, item);
        }

        public void RemoveAt(int index)
        {
            IElement oldItem = this.elements[index];
            this.elements.RemoveAt(index);
            this.owner.InvalidateMeasure();
            this.SetParents(oldItem, null);
        }

        public void Add(object item, Func<IElement> template)
        {
            this.Add(Realize(item, template));
        }

        public void Insert(int index, object item, Func<IElement> template)
        {
            this.Insert(index, Realize(item, template));
        }

        public void Move(int oldIndex, int newIndex)
        {
            IElement element = this[oldIndex];
            this.RemoveAt(oldIndex);
            this.Insert(newIndex, element);
        }

        private static IElement Realize(object item, Func<IElement> template)
        {
            if (template == null)
            {
                throw new InvalidOperationException("An element cannot be created without a template");
            }

            IElement element = template();
            element.DataContext = item;
            return element;
        }

        private void SetParents(IElement oldItem, IElement newItem)
        {
            if (oldItem != null)
            {
                oldItem.VisualParent = null;
            }

            if (newItem != null)
            {
                newItem.VisualParent = this.owner;
            }
        }
    }
}