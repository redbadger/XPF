namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class VirtualizingElementCollection : IList<IElement>, ITemplatedList<IElement>
    {
        public int Count
        {
            get
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReal(int index)
        {
            return true;
        }

        public void Virtualize(int index)
        {
        }

        public void Add(IElement element)
        {
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(IElement item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IElement[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IElement item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<IElement> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(IElement item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, IElement item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public virtual void Add(object item, Func<object, IElement> template)
        {
            this.Add(NewContainer(item, template));
        }

        public virtual void Insert(int index, object item, Func<object, IElement> template)
        {
            this.Insert(index, NewContainer(item, template));
        }

        public virtual void Move(int oldIndex, int newIndex)
        {
            var element = this[oldIndex];
            this.RemoveAt(oldIndex);
            this.Insert(newIndex, element);
        }

        protected static IElement NewContainer(object item, Func<object, IElement> template)
        {
            if (template == null)
            {
                throw new InvalidOperationException("A Template for this Item has not been supplied");
            }

            var element = template(item);
            element.DataContext = item;
            return element;
        }
    }
}