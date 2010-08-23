namespace RedBadger.Xpf.Presentation
{
    using System;

#if WINDOWS_PHONE
    public interface ITemplatedList<T>
#else
    public interface ITemplatedList<in T>
#endif
    {
        void Add(object item, Func<IElement> template);

        void Clear();

        void Insert(int index, object item, Func<IElement> template);

        void Move(int oldIndex, int newIndex);

        void RemoveAt(int index);
    }
}