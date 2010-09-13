namespace RedBadger.Xpf.Presentation.Data
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    /// <summary>
    ///     A factory that creates IObservable and IObserver around a variety of sources.
    /// </summary>
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
        /// <returns><see cref = "IObservable{T}">IObservable</see> around the Data Context.</returns>
        public static IObservable<TSource> CreateOneWay<TSource>()
        {
            return new OneWayBinding<TSource>();
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
        /// <returns><see cref = "IObservable{T}">IObservable</see> around the property on the Data Context.</returns>
        public static IObservable<TProperty> CreateOneWay<TSource, TProperty>(
            Expression<Func<TSource, TProperty>> propertySelector)
        {
            return new OneWayBinding<TProperty>(GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a One Way Binding to a <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> on the element's Data Context.
        /// </summary>
        /// <remarks>
        ///     When binding to the Data Context, the binding returned is not resolved (ie. connected to the source) until the beginning of the Measure phase.
        ///     This allows for the Data Context to be set after the binding has been created and changed at any time.
        /// </remarks>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "TProperty">The Type of the property on the Data Context.</typeparam>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> you're binding to.</param>
        /// <returns><see cref = "IObservable{T}">IObservable</see> around the property on the Data Context.</returns>
        public static IObservable<TProperty> CreateOneWay<TSource, TProperty>(
            ReactiveProperty<TProperty, TSource> reactiveProperty) where TSource : ReactiveObject
        {
            return new OneWayReactivePropertyBinding<TSource, TProperty>(reactiveProperty);
        }

        /// <summary>
        ///     Creates a One Way Binding directly to a source object.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <returns>IObservable around the source.</returns>
        public static IObservable<TSource> CreateOneWay<TSource>(TSource source)
        {
            return new OneWayBinding<TSource>(source);
        }

        /// <summary>
        ///     Creates a One Way Binding to a property on a source.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "TProperty">The Type of the property on the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "propertySelector">Lambda expression which returns the property on the source.</param>
        /// <returns><see cref = "IObservable{T}">IObservable</see> around the property on the source.</returns>
        public static IObservable<TProperty> CreateOneWay<TSource, TProperty>(
            TSource source, Expression<Func<TSource, TProperty>> propertySelector)
        {
            return new OneWayBinding<TProperty>(source, GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a One Way Binding to a <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> on a source.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "TProperty">The Type of the property on the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> on the source.</param>
        /// <returns><see cref = "IObservable{T}">IObservable</see> around the property on the source.</returns>
        public static IObservable<TProperty> CreateOneWay<TSource, TProperty>(
            TSource source, ReactiveProperty<TProperty, TSource> reactiveProperty) where TSource : ReactiveObject
        {
            return new OneWayReactivePropertyBinding<TSource, TProperty>(source, reactiveProperty);
        }

        /// <summary>
        ///     Creates a One Way To Source Binding to a property on the Data Context.
        /// </summary>
        /// <remarks>
        ///     When binding to the Data Context, the binding returned is not resolved (ie. connected to the source) until the beginning of the Measure phase.
        ///     This allows for the Data Context to be set after the binding has been created and changed at any time.
        /// </remarks>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "TProperty">The Type of the property on the Data Context.</typeparam>
        /// <param name = "propertySelector">Lambda expression which returns the property on the Data Context.</param>
        /// <returns><see cref = "IObserver{T}">IObserver</see> around the property on the Data Context.</returns>
        public static IObserver<TProperty> CreateOneWayToSource<TSource, TProperty>(
            Expression<Func<TSource, TProperty>> propertySelector)
        {
            return new OneWayToSourceBinding<TProperty>(GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a One Way To Source Binding to a <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> on the Data Context.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "TProperty">The Type of the property on the Data Context.</typeparam>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> you're binding to.</param>
        /// <returns><see cref = "IObserver{T}">IObserver</see> around the property on the Data Context.</returns>
        public static IObserver<TProperty> CreateOneWayToSource<TSource, TProperty>(
            ReactiveProperty<TProperty, TSource> reactiveProperty) where TSource : ReactiveObject
        {
            return new OneWayToSourceReactivePropertyBinding<TSource, TProperty>(reactiveProperty);
        }

        /// <summary>
        ///     Creates a One Way To Source Binding to a property on a source.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "TProperty">The Type of the property on the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "propertySelector">Lambda expression which returns the property on the source.</param>
        /// <returns><see cref = "IObserver{T}">IObserver</see> around the property on the source.</returns>
        public static IObserver<TProperty> CreateOneWayToSource<TSource, TProperty>(
            TSource source, Expression<Func<TSource, TProperty>> propertySelector)
        {
            return new OneWayToSourceBinding<TProperty>(source, GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a One Way To Source Binding to a <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> on a source.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "TProperty">The Type of the property on the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> on the source.</param>
        /// <returns><see cref = "IObserver{T}">IObserver</see> around the property on the source.</returns>
        public static IObserver<TProperty> CreateOneWayToSource<TSource, TProperty>(
            TSource source, ReactiveProperty<TProperty, TSource> reactiveProperty) where TSource : ReactiveObject
        {
            return new OneWayToSourceReactivePropertyBinding<TSource, TProperty>(source, reactiveProperty);
        }

        /// <summary>
        ///     Creates a Two Way Binding to a property on the element's Data Context.
        /// </summary>
        /// <remarks>
        ///     When binding to the Data Context, the binding returned is not resolved (ie. connected to the source) until the beginning of the Measure phase.
        ///     This allows for the Data Context to be set after the binding has been created and changed at any time.
        /// </remarks>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "TProperty">The Type of the property on the Data Context.</typeparam>
        /// <param name = "propertySelector">Lambda expression which returns the property on the Data Context.</param>
        /// <returns><see cref = "IDualChannel{T}">IDualChannel</see> around the property on the Data Context.</returns>
        public static IDualChannel<TProperty> CreateTwoWay<TSource, TProperty>(
            Expression<Func<TSource, TProperty>> propertySelector)
        {
            return new TwoWayBinding<TProperty>(GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a Two Way Binding to a <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> on the element's Data Context.
        /// </summary>
        /// <remarks>
        ///     When binding to the Data Context, the binding returned is not resolved (ie. connected to the source) until the beginning of the Measure phase.
        ///     This allows for the Data Context to be set after the binding has been created and changed at any time.
        /// </remarks>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "TProperty">The Type of the <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> on the Data Context.</typeparam>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> you're binding to.</param>
        /// <returns><see cref = "IDualChannel{T}">IDualChannel</see> around the property on the source.</returns>
        public static IDualChannel<TProperty> CreateTwoWay<TSource, TProperty>(
            ReactiveProperty<TProperty, TSource> reactiveProperty) where TSource : ReactiveObject
        {
            return new TwoWayReactivePropertyBinding<TSource, TProperty>(reactiveProperty);
        }

        /// <summary>
        ///     Creates a Two Way Binding to a property on a source.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "TProperty">The Type of the property on the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "propertySelector">Lambda expression which returns the property on the source.</param>
        /// <returns><see cref = "IDualChannel{T}">IDualChannel</see> around the property on the source.</returns>
        public static IDualChannel<TProperty> CreateTwoWay<TSource, TProperty>(
            TSource source, Expression<Func<TSource, TProperty>> propertySelector)
        {
            return new TwoWayBinding<TProperty>(source, GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a Two Way Binding to a <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> on a source.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "TProperty">The Type of the <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> on the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{TProperty,TOwner}">ReactiveProperty</see> on the source.</param>
        /// <returns><see cref = "IDualChannel{T}">IDualChannel</see> around the property on the source.</returns>
        public static IDualChannel<TProperty> CreateTwoWay<TSource, TProperty>(
            TSource source, ReactiveProperty<TProperty, TSource> reactiveProperty) where TSource : ReactiveObject
        {
            return
                new TwoWayReactivePropertyBinding<TSource, TProperty>(source, reactiveProperty);
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