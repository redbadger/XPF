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

        private readonly IElement owner;

        public VirtualizingElementCollection(IElement owner)
        {
            this.owner = owner;
            this.cursor = new Cursor(this.items);
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
            this.items.Add(new Memento(item, template, this.owner));
        }

        public virtual void Insert(int index, object item, Func<IElement> template)
        {
            this.items.Insert(index, new Memento(item, template, this.owner));
        }

        public virtual void Move(int oldIndex, int newIndex)
        {
            var memento = this.items[oldIndex];
            this.items.RemoveAt(oldIndex);
            this.items.Insert(newIndex, memento);
        }

        public class Cursor : IDisposable, IEnumerable<IElement>
        {
            private readonly IList<Memento> mementoes;

            private LinkedList<Memento> currentRealizedMementoes = new LinkedList<Memento>();

            private int firstMemento;

            private bool isDisposed;

            private LinkedList<Memento> previousRealizedMementoes = new LinkedList<Memento>();

            public Cursor(IList<Memento> mementoes)
            {
                this.mementoes = mementoes;
            }

            public IEnumerable<IElement> CurrentlyRealized
            {
                get
                {
                    return this.previousRealizedMementoes.Select(memento => memento.Element);
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

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public IEnumerator<IElement> GetEnumerator()
            {
                for (int i = this.firstMemento; i < this.mementoes.Count; i++)
                {
                    var memento = this.mementoes[i];
                    var element = memento.IsReal ? memento.Element : memento.Realize();

                    this.currentRealizedMementoes.AddLast(memento);
                    yield return element;
                }
            }
        }

        public class Memento
        {
            private readonly object item;

            private readonly IElement owner;

            private readonly Func<IElement> template;

            private IElement element;

            public Memento(object item, Func<IElement> template, IElement owner)
            {
                if (template == null)
                {
                    throw new InvalidOperationException("A Template for this Item has not been supplied");
                }

                this.item = item;
                this.template = template;
                this.owner = owner;

                this.owner.InvalidateMeasure();
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

            public IElement Realize()
            {
                this.element = this.Create();
                this.element.VisualParent = this.owner;
                this.owner.InvalidateMeasure();
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