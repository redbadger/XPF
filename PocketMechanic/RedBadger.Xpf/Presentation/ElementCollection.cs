namespace RedBadger.Xpf.Presentation
{
    using System.Collections;
    using System.Collections.Generic;

    public class ElementCollection : IList<IElement>
    {
        private readonly List<IElement> children = new List<IElement>();

        private readonly IElement owner;

        public ElementCollection(IElement owner)
        {
            this.owner = owner;
        }

        public int Count
        {
            get
            {
                return this.children.Count;
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
                return this.children[index];
            }

            set
            {
                IElement oldItem = this.children[index];
                IElement newItem = value;

                this.children[index] = newItem;
                this.owner.InvalidateMeasure();
                this.SetParents(oldItem, newItem);
            }
        }

        public void Add(IElement item)
        {
            this.children.Add(item);
            this.owner.InvalidateMeasure();
            this.SetParents(null, item);
        }

        public void Clear()
        {
            this.children.Clear();
        }

        public bool Contains(IElement item)
        {
            return this.children.Contains(item);
        }

        public void CopyTo(IElement[] array, int arrayIndex)
        {
            this.children.CopyTo(array, arrayIndex);
        }

        public bool Remove(IElement item)
        {
            bool wasRemoved = this.children.Remove(item);
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
            return this.children.GetEnumerator();
        }

        public int IndexOf(IElement item)
        {
            return this.children.IndexOf(item);
        }

        public void Insert(int index, IElement item)
        {
            this.children.Insert(index, item);
            this.owner.InvalidateMeasure();
            this.SetParents(null, item);
        }

        public void RemoveAt(int index)
        {
            var oldItem = this.children[index];
            this.children.RemoveAt(index);
            this.owner.InvalidateMeasure();
            this.SetParents(oldItem, null);
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