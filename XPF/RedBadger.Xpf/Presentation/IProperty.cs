namespace RedBadger.Xpf.Presentation
{
    using System;

    public interface IProperty
    {
        object DefaultValue { get; }

        string Name { get; }

        Type PropertyType { get; }
    }
}