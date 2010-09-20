namespace RedBadger.Xpf.Internal
{
    using System;
    using System.Collections.Generic;

    using RedBadger.Xpf.Presentation;

    internal class PropertyStore<T> : Dictionary<Type, Dictionary<string, ReactiveProperty<T>>>
    {
    }
}