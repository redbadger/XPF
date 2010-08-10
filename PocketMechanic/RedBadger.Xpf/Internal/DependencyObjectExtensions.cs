namespace RedBadger.Xpf.Internal
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;

    public static class DependencyObjectExtensions
    {
        public static IObservable<T> FromDependencyProperty<T>(
            this DependencyObject target, DependencyProperty property)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            return Observable.Create<T>(
                observer =>
                    {
                        DependencyPropertyDescriptor propertyDescriptor =
                            DependencyPropertyDescriptor.FromProperty(property, target.GetType());
                        var handler =
                            new EventHandler((sender, e) => observer.OnNext((T)propertyDescriptor.GetValue(target)));
                        propertyDescriptor.AddValueChanged(target, handler);
                        return () => propertyDescriptor.RemoveValueChanged(target, handler);
                    });
        }
    }
}