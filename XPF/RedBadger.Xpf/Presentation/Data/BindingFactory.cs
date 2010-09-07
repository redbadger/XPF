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
            TSource source, Expression<Func<TSource, TProperty>> propertySelector)
            where TSource : INotifyPropertyChanged
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertySelector);

            Func<TProperty> getValue = () => (TProperty)propertyInfo.GetValue(source, null);

            var subject = new BehaviorSubject<TProperty>(getValue());

            GetObservable(source, propertyInfo).Subscribe(data => subject.OnNext(getValue()));
            return subject.AsObservable();
        }

        public static IObservable<TSource> CreateOneWay<TSource>(TSource source)
        {
            return new BehaviorSubject<TSource>(source).AsObservable();
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
            PropertyInfo propertyInfo = GetPropertyInfo(propertySelector);

            Func<TProperty> getValue = () => (TProperty)propertyInfo.GetValue(source, null);

            var observable = new BehaviorSubject<TProperty>(getValue());
            GetObservable(source, propertyInfo).Subscribe(data => observable.OnNext(getValue()));

            var observer = new Subject<TProperty>();
            observer.Subscribe(value => GetPropertyInfo(propertySelector).SetValue(source, value, null));

            return new TwoWayBinding<TProperty>(observable.AsObservable(), observer.AsObserver());
        }

        private static IObservable<IEvent<PropertyChangedEventArgs>> GetObservable<TSource>(
            TSource source, PropertyInfo propertyInfo)
        {
            return
                Observable.FromEvent<PropertyChangedEventArgs>(
                    handler => ((INotifyPropertyChanged)source).PropertyChanged += handler, 
                    handler => ((INotifyPropertyChanged)source).PropertyChanged -= handler).Where(
                        data => data.EventArgs.PropertyName == propertyInfo.Name);
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