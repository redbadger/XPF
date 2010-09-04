namespace RedBadger.Xpf.Presentation
{
    using System;

    public interface IDependencyProperty
    {
        object DefaultValue { get; }

        string Name { get; }

        Action<DependencyObject, DependencyPropertyChangedEventArgs> PropertyChangedCallback { get; }

        Type PropertyType { get; }
    }
}