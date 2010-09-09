namespace RedBadger.Xpf.Presentation.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    public static class BindingFactory
    {
        public static IObservable<TProperty> CreateOneWay<TSource, TProperty>(
            Expression<Func<TSource, TProperty>> propertySelector)
            where TSource : INotifyPropertyChanged
        {
            return new DeferredBinding<TProperty>(GetPropertyInfo(propertySelector));
        }

        public static IObservable<TProperty> CreateOneWay<TSource, TProperty>(
            TSource source, Expression<Func<TSource, TProperty>> propertySelector)
            where TSource : INotifyPropertyChanged
        {
            return GetObservable<TProperty>(source, GetPropertyInfo(propertySelector));
        }

        public static IObservable<TProperty> CreateOneWay<TSource, TProperty>(
            TSource source, ReactiveProperty<TProperty, TSource> property) where TSource : DependencyObject
        {
            return source.GetObservable(property);
        }

        public static IObservable<TSource> CreateOneWay<TSource>(TSource source)
        {
            return new BehaviorSubject<TSource>(source).AsObservable();
        }

        public static IObserver<TProperty> CreateOneWayToSource<TSource, TProperty>(
            TSource source, ReactiveProperty<TProperty, TSource> property) where TSource : DependencyObject
        {
            return source.GetObserver(property);
        }

        public static IObserver<TProperty> CreateOneWayToSource<TSource, TProperty>(
            TSource source, Expression<Func<TSource, TProperty>> propertySelector)
        {
            var subject = new Subject<TProperty>();
            subject.Subscribe(value => GetPropertyInfo(propertySelector).SetValue(source, value, null));
            return subject.AsObserver();
        }

        public static TwoWayBinding<TProperty> CreateTwoWay<TSource, TProperty>(
            TSource source, Expression<Func<TSource, TProperty>> propertySelector)
            where TSource : INotifyPropertyChanged
        {
            var observer = new Subject<TProperty>();
            observer.Subscribe(value => GetPropertyInfo(propertySelector).SetValue(source, value, null));

            return new TwoWayBinding<TProperty>(
                GetObservable<TProperty>(source, GetPropertyInfo(propertySelector)), observer.AsObserver());
        }

        public static IObservable<TProperty> GetObservable<TProperty>(
            INotifyPropertyChanged source, PropertyInfo propertyInfo)
        {
            return
                Observable.FromEvent<PropertyChangedEventArgs>(
                    handler => source.PropertyChanged += handler, handler => source.PropertyChanged -= handler).Where(
                        data => data.EventArgs.PropertyName == propertyInfo.Name).Select(
                            e => (TProperty)propertyInfo.GetValue(source, null));
        }

        private static PropertyInfo GetPropertyInfo<TSource, TProperty>(
            Expression<Func<TSource, TProperty>> propertySelector)
        {
            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException();
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException();
            }

            return propertyInfo;
        }
    }
}