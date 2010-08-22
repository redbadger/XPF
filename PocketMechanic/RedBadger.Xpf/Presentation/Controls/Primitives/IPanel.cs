namespace RedBadger.Xpf.Presentation.Controls.Primitives
{
    using System;

    public interface IPanel
    {
        void AddChild(object newItem, Func<object, IElement> containerGenerator);

        void RemoveChildAt(int index);

        void InsertChildAt(int index, object newItem, Func<object, IElement> containerGenerator);

        void MoveChild(int oldIndex, int newIndex);

        void ClearChildren();
    }
}