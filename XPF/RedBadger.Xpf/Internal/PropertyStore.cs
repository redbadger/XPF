namespace RedBadger.Xpf.Internal
{
    using System;
    using System.Collections.Generic;

    using RedBadger.Xpf.Presentation;

    internal class PropertyStore<TProperty, TOwner> :
        Dictionary<Type, Dictionary<string, Property<TProperty, TOwner>>> where TOwner : class
    {
    }
}