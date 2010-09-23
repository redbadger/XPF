namespace RedBadger.Xpf.Internal
{
    using System;
    using System.Collections.Generic;

    internal class PropertyStore<T> : Dictionary<Type, Dictionary<string, ReactiveProperty<T>>>
    {
    }
}