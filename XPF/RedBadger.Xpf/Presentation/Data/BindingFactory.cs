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
        /// <summary>
        ///     Creates a One Way Binding to the element's Data Context.
        /// </summary>
        /// <remarks>
        ///     When binding to the Data Context, the binding returned is not resolved (ie. connected to the source) until the beginning of the Measure phase.
        ///     This allows for the Data Context to be set after the binding has been created and changed at any time.
        /// </remarks>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <returns>IObservable around the Data Context.</returns>
        public static IObservable<TSource> CreateOneWay<TSource>()
        {
            return new Binding<TSource>();
        }

        /// <summary>
        ///     Creates a One Way Binding to a property on the element's Data Context.
        /// </summary>
        /// <remarks>
        ///     When binding to the Data Context, the binding returned is not resolved (ie. connected to the source) until the beginning of the Measure phase.
        ///     This allows for the Data Context to be set after the binding has been created and changed at any time.
        /// </remarks>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "TProperty">The Type of the property on the Data Context.</typeparam>
        /// <param name = "propertySelector">Lambda expression which returns the property on the Data Context.</param>
        /// <returns>IObservable around the property on the Data Context.</returns>
        public static IObservable<TProperty> CreateOneWay<TSource, TProperty>(
            Expression<Func<TSource, TProperty>> propertySelector)
        {
            return new Binding<TProperty>(GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a One Way Binding to a ReactiveProperty on the element's Data Context.
        /// </summary>
        /// <remarks>
        ///     When binding to the Data Context, the binding returned is not resolved (ie. connected to the source) until the beginning of the Measure phase.
        ///     This allows for the Data Context to be set after the binding has been created and changed at any time.
        /// </remarks>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "TProperty">The Type of the property on the Data Context.</typeparam>
        /// <param name = "reactiveProperty">The ReactiveProperty you're binding to.</param>
        /// <returns>IObservable around the property on the Data Context.</returns>
        public static IObservable<TProperty> CreateOneWay<TSource, TProperty>(
            ReactiveProperty<TProperty, TSource> reactiveProperty) where TSource : DependencyObject
        {
            return new ReactivePropertyBinding<TSource, TProperty>(reactiveProperty);
        }

        /// <summary>
        ///     Creates a One Way Binding directly to a source object.
        /// </summary>
        /// <typeparam name = "TSource"></typeparam>
        /// <param name = "source"></param>
        /// <returns></returns>
        public static IObservable<TSource> CreateOneWay<TSource>(TSource source)
        {
            return new Binding<TSource>(source);
        }

        /// <summary>
        ///     Creates a One Way Binding to a property on a source.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "TProperty">The Type of the property on the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "propertySelector">Lambda expression which returns the property on the source.</param>
        /// <returns>IObservable around the property on the source.</returns>
        public static IObservable<TProperty> CreateOneWay<TSource, TProperty>(
            TSource source, Expression<Func<TSource, TProperty>> propertySelector)
        {
            return new Binding<TProperty>(source, GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a One Way Binding to a ReactiveProperty on a source.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "TProperty">The Type of the property on the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> on the source.</param>
        /// <returns>IObservable around the property on the source.</returns>
        public static IObservable<TProperty> CreateOneWay<TSource, TProperty>(
            TSource source, ReactiveProperty<TProperty, TSource> reactiveProperty) where TSource : DependencyObject
        {
            return new ReactivePropertyBinding<TSource, TProperty>(source, reactiveProperty);
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