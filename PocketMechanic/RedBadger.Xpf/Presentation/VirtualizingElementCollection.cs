namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class VirtualizingElementCollection : IList<IElement>, ITemplatedList<IElement>
    {
        private readonly Cursor cursor;

        private readonly IList<Memento> items = new List<Memento>();

        public VirtualizingElementCollection(IElement owner)
        {
            this.cursor = new Cursor(this.items, owner);
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

        public IEnumerable<IElement> RealizedElements
        {
            get
            {
                return this.cursor.CurrentlyRealized;
            }
        }

        public IElement this[int index]
        {
            get
            {
                var memento = this.items[index];
                return memento.IsReal ? memento.Element : memento.Create();
            }

            set
            {
                throw new NotSupportedException("Please use the overload that takes a template");
            }
        }

        public Cursor GetCursor(int startIndex)
        {
            return this.cursor.UnDispose(startIndex);
        }

        public bool IsReal(int index)
        {
            return this.items[index].IsReal;
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

        public virtual void Add(object item, Func<IElement> template)
        {
            this.items.Add(new Memento(item, template));
        }

        public virtual void Insert(int index, object item, Func<IElement> template)
        {
            this.items.Insert(index, new Memento(item, template));
        }

        public virtual void Move(int oldIndex, int newIndex)
        {
            var memento = this.items[oldIndex];
            this.items.RemoveAt(oldIndex);
            this.items.Insert(newIndex, memento);
        }

        public class Cursor : IDisposable
        {
            private readonly IList<Memento> mementoes;

            private readonly IElement owner;

            private LinkedList<Memento> currentRealizedMementoes = new LinkedList<Memento>();

            private int firstMemento;

            private bool isDisposed;

            private LinkedList<Memento> previousRealizedMementoes = new LinkedList<Memento>();

            public Cursor(IList<Memento> mementoes, IElement owner)
            {
                this.mementoes = mementoes;
                this.owner = owner;
            }

            public IEnumerable<IElement> CurrentlyRealized
            {
                get
                {
                    return this.previousRealizedMementoes.Select(memento => memento.Element);
                }
            }

            public IEnumerable<IElement> Items
            {
                get
                {
                    for (int i = this.firstMemento; i < this.mementoes.Count; i++)
                    {
                        var memento = this.mementoes[i];
                        var element = memento.IsReal ? memento.Element : memento.Realize(this.owner);

                        this.currentRealizedMementoes.AddLast(memento);
                        yield return element;
                    }
                }
            }

            public void Dispose(bool isDisposing)
            {
                this.isDisposed = true;
                foreach (var memento in this.previousRealizedMementoes.Except(this.currentRealizedMementoes))
                {
                    memento.Virtualize();
                }

                this.previousRealizedMementoes.Clear();
                var newCurrent = this.previousRealizedMementoes;
                this.previousRealizedMementoes = this.currentRealizedMementoes;
                this.currentRealizedMementoes = newCurrent;
            }

            public Cursor UnDispose(int startIndex)
            {
                this.isDisposed = false;
                this.firstMemento = startIndex;
                return this;
            }

            public void Dispose()
            {
                if (!this.isDisposed)
                {
                    this.Dispose(true);
                }
            }
        }

        public class Memento
        {
            private readonly object item;

            private readonly Func<IElement> template;

            private IElement element;

            public Memento(object item, Func<IElement> template)
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
                var newElement = this.template();
                newElement.DataContext = this.item;
                return newElement;
            }

            public IElement Realize(IElement owner)
            {
                this.element = this.Create();
                this.element.VisualParent = owner;
                owner.InvalidateMeasure();
                return this.element;
            }

            public void Virtualize()
            {
                this.element.DataContext = null;
                this.element.VisualParent = null;
                this.element = null;
            }
        }
    }
}