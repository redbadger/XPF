namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;

    public class DependencyPropertyStore<TProperty, TOwner> :
        Dictionary<Type, Dictionary<string, Property<TProperty, TOwner>>> where TOwner : class
    {
    }
}