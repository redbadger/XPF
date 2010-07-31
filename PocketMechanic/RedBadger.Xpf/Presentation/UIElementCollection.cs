namespace RedBadger.Xpf.Presentation
{
    using System.Collections;
    using System.Collections.Generic;

    public class UIElementCollection : IList<UIElement>
    {
        private readonly List<UIElement> children = new List<UIElement>();

        private readonly IElement owner;

        public UIElementCollection(IElement owner)
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

        public UIElement this[int index]
        {
            get
            {
                return this.children[index];
            }

            set
            {
                UIElement oldItem = this.children[index];
                UIElement newItem = value;

                this.children[index] = newItem;
                this.SetParents(oldItem, newItem);
            }
        }

        public void Add(UIElement item)
        {
            this.children.Add(item);
            this.SetParents(null, item);
        }

        public void Clear()
        {
            this.children.Clear();
        }

        public bool Contains(UIElement item)
        {
            return this.children.Contains(item);
        }

        public void CopyTo(UIElement[] array, int arrayIndex)
        {
            this.children.CopyTo(array, arrayIndex);
        }

        public bool Remove(UIElement item)
        {
            bool wasRemoved = this.children.Remove(item);
            if (wasRemoved)
            {
                this.SetParents(item, null);
            }

            return wasRemoved;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<UIElement> GetEnumerator()
        {
            return this.children.GetEnumerator();
        }

        public int IndexOf(UIElement item)
        {
            return this.children.IndexOf(item);
        }

        public void Insert(int index, UIElement item)
        {
            this.children.Insert(index, item);
            this.SetParents(null, item);
        }

        public void RemoveAt(int index)
        {
            var oldItem = this.children[index];
            this.children.RemoveAt(index);
            this.SetParents(oldItem, null);
        }

        private void SetParents(UIElement oldItem, UIElement newItem)
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