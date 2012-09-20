#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

namespace RedBadger.Xpf.Data
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

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
        /// <typeparam name = "T">The Type of the property on the Data Context.</typeparam>
        /// <param name = "propertySelector">Lambda expression which returns the property on the Data Context.</param>
        /// <returns><see cref = "IObservable{T}">IObservable</see> around the property on the Data Context.</returns>
        public static IObservable<T> CreateOneWay<TSource, T>(Expression<Func<TSource, T>> propertySelector)
        {
            return new OneWayBinding<T>(GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a One Way Binding to a property on the element's Data Context
        ///     where the Type of the source property is different from the Type of the target property and requires conversion.
        /// </summary>
        /// <remarks>
        ///     When binding to the Data Context, the binding returned is not resolved (ie. connected to the source) until the beginning of the Measure phase.
        ///     This allows for the Data Context to be set after the binding has been created and changed at any time.
        /// </remarks>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "TSourceProp">The Type of the property on the Data Context.</typeparam>
        /// <typeparam name = "TTargetProp">The Type of the property on the target.</typeparam>
        /// <param name = "propertySelector">Lambda expression which returns the property on the Data Context.</param>
        /// <returns><see cref = "IObservable{T}">IObservable</see> around the property on the Data Context.</returns>
        public static IObservable<TTargetProp> CreateOneWay<TSource, TSourceProp, TTargetProp>(
            Expression<Func<TSource, TSourceProp>> propertySelector)
        {
            return new OneWayBinding<TTargetProp>(GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a One Way Binding to a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on the element's Data Context.
        /// </summary>
        /// <remarks>
        ///     When binding to the Data Context, the binding returned is not resolved (ie. connected to the source) until the beginning of the Measure phase.
        ///     This allows for the Data Context to be set after the binding has been created and changed at any time.
        /// </remarks>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "T">The Type of the property on the Data Context.</typeparam>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> you're binding to.</param>
        /// <returns><see cref = "IObservable{T}">IObservable</see> around the property on the Data Context.</returns>
        public static IObservable<T> CreateOneWay<TSource, T>(ReactiveProperty<T> reactiveProperty)
            where TSource : ReactiveObject
        {
            return new OneWayReactivePropertyBinding<T, TSource>(reactiveProperty);
        }

        /// <summary>
        ///     Creates a One Way Binding to a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on the element's Data Context.
        ///     where the Type of the source property is different from the Type of the target property and requires conversion.
        /// </summary>
        /// <remarks>
        ///     When binding to the Data Context, the binding returned is not resolved (ie. connected to the source) until the beginning of the Measure phase.
        ///     This allows for the Data Context to be set after the binding has been created and changed at any time.
        /// </remarks>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "TSourceProp">The Type of the property on the source Data Context.</typeparam>
        /// <typeparam name = "TTargetProp">The Type of the property on the target.</typeparam>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> you're binding to.</param>
        /// <returns><see cref = "IObservable{T}">IObservable</see> around the property on the Data Context.</returns>
        public static IObservable<TTargetProp> CreateOneWay<TSource, TSourceProp, TTargetProp>(
            ReactiveProperty<TSourceProp> reactiveProperty) where TSource : ReactiveObject
        {
            return new OneWayReactivePropertyBinding<TTargetProp, TSourceProp, TSource>(reactiveProperty);
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
        ///     Creates a One Way Binding directly to a source object
        ///     where the Type of the source is different from the Type of the target property and requires conversion.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "TTargetProp">The Type of the property on the target.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <returns>IObservable around the source.</returns>
        public static IObservable<TTargetProp> CreateOneWay<TSource, TTargetProp>(TSource source)
        {
            return new OneWayBinding<TTargetProp>(OneWayBinding<TTargetProp>.Convert(source));
        }

        /// <summary>
        ///     Creates a One Way Binding to a property on a source.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "T">The Type of the property on the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "propertySelector">Lambda expression which returns the property on the source.</param>
        /// <returns><see cref = "IObservable{T}">IObservable</see> around the property on the source.</returns>
        public static IObservable<T> CreateOneWay<TSource, T>(
            TSource source, Expression<Func<TSource, T>> propertySelector)
        {
            return new OneWayBinding<T>(source, GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a One Way Binding to a property on a source
        ///     where the Type of the source property is different from the Type of the target property and requires conversion.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "TSourceProp">The Type of the property on the source.</typeparam>
        /// <typeparam name = "TTargetProp">The Type of the property on the target.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "propertySelector">Lambda expression which returns the property on the source.</param>
        /// <returns><see cref = "IObservable{T}">IObservable</see> around the property on the source.</returns>
        public static IObservable<TTargetProp> CreateOneWay<TSource, TSourceProp, TTargetProp>(
            TSource source, Expression<Func<TSource, TSourceProp>> propertySelector)
        {
            return new OneWayBinding<TTargetProp>(source, GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a One Way Binding to a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on a source.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "T">The Type of the property on the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on the source.</param>
        /// <returns><see cref = "IObservable{T}">IObservable</see> around the property on the source.</returns>
        public static IObservable<T> CreateOneWay<TSource, T>(TSource source, ReactiveProperty<T> reactiveProperty)
            where TSource : ReactiveObject
        {
            return new OneWayReactivePropertyBinding<T, TSource>(source, reactiveProperty);
        }

        /// <summary>
        ///     Creates a One Way Binding to a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on a source
        ///     where the Type of the source property is different from the Type of the target property and requires conversion.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "TSourceProp">The Type of the property on the source.</typeparam>
        /// <typeparam name = "TTargetProp">The Type of the property on the target.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on the source.</param>
        /// <returns><see cref = "IObservable{T}">IObservable</see> around the property on the source.</returns>
        public static IObservable<TTargetProp> CreateOneWay<TSource, TSourceProp, TTargetProp>(
            TSource source, ReactiveProperty<TSourceProp> reactiveProperty) where TSource : ReactiveObject
        {
            return new OneWayReactivePropertyBinding<TTargetProp, TSourceProp, TSource>(source, reactiveProperty);
        }

        /// <summary>
        ///     Creates a One Way To Source Binding to a property on the Data Context.
        /// </summary>
        /// <remarks>
        ///     When binding to the Data Context, the binding returned is not resolved (ie. connected to the source) until the beginning of the Measure phase.
        ///     This allows for the Data Context to be set after the binding has been created and changed at any time.
        /// </remarks>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "T">The Type of the property on the Data Context.</typeparam>
        /// <param name = "propertySelector">Lambda expression which returns the property on the Data Context.</param>
        /// <returns><see cref = "IObserver{T}">IObserver</see> around the property on the Data Context.</returns>
        public static IObserver<T> CreateOneWayToSource<TSource, T>(Expression<Func<TSource, T>> propertySelector)
        {
            return new OneWayToSourceBinding<T>(GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a One Way To Source Binding to a property on the Data Context
        ///     where the Type of the source property is different from the Type of the target property and requires conversion.
        /// </summary>
        /// <remarks>
        ///     When binding to the Data Context, the binding returned is not resolved (ie. connected to the source) until the beginning of the Measure phase.
        ///     This allows for the Data Context to be set after the binding has been created and changed at any time.
        /// </remarks>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "TSourceProp">The Type of the property on the source Data Context.</typeparam>
        /// <typeparam name = "TTargetProp">The Type of the property on the target.</typeparam>
        /// <param name = "propertySelector">Lambda expression which returns the property on the Data Context.</param>
        /// <returns><see cref = "IObserver{T}">IObserver</see> around the property on the Data Context.</returns>
        public static IObserver<TTargetProp> CreateOneWayToSource<TSource, TSourceProp, TTargetProp>(
            Expression<Func<TSource, TSourceProp>> propertySelector)
        {
            return new OneWayToSourceBinding<TTargetProp>(GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a One Way To Source Binding to a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on the Data Context.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "T">The Type of the property on the Data Context.</typeparam>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> you're binding to.</param>
        /// <returns><see cref = "IObserver{T}">IObserver</see> around the property on the Data Context.</returns>
        public static IObserver<T> CreateOneWayToSource<TSource, T>(ReactiveProperty<T> reactiveProperty)
            where TSource : ReactiveObject
        {
            return new OneWayToSourceReactivePropertyBinding<T, TSource>(reactiveProperty);
        }

        /// <summary>
        ///     Creates a One Way To Source Binding to a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on the Data Context
        ///     where the Type of the source property is different from the Type of the target property and requires conversion.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "TSourceProp">The Type of the property on the source Data Context.</typeparam>
        /// <typeparam name = "TTargetProp">The Type of the property on the target.</typeparam>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> you're binding to.</param>
        /// <returns><see cref = "IObserver{T}">IObserver</see> around the property on the Data Context.</returns>
        public static IObserver<TTargetProp> CreateOneWayToSource<TSource, TSourceProp, TTargetProp>(
            ReactiveProperty<TSourceProp> reactiveProperty) where TSource : ReactiveObject
        {
            return new OneWayToSourceReactivePropertyBinding<TTargetProp, TSourceProp, TSource>(reactiveProperty);
        }

        /// <summary>
        ///     Creates a One Way To Source Binding to a property on a source.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "T">The Type of the property on the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "propertySelector">Lambda expression which returns the property on the source.</param>
        /// <returns><see cref = "IObserver{T}">IObserver</see> around the property on the source.</returns>
        public static IObserver<T> CreateOneWayToSource<TSource, T>(
            TSource source, Expression<Func<TSource, T>> propertySelector)
        {
            return new OneWayToSourceBinding<T>(source, GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a One Way To Source Binding to a property on a source
        ///     where the Type of the source property is different from the Type of the target property and requires conversion.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "TSourceProp">The Type of the property on the source.</typeparam>
        /// <typeparam name = "TTargetProp">The Type of the property on the target.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "propertySelector">Lambda expression which returns the property on the source.</param>
        /// <returns><see cref = "IObserver{T}">IObserver</see> around the property on the source.</returns>
        public static IObserver<TTargetProp> CreateOneWayToSource<TSource, TSourceProp, TTargetProp>(
            TSource source, Expression<Func<TSource, TSourceProp>> propertySelector)
        {
            return new OneWayToSourceBinding<TTargetProp>(source, GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a One Way To Source Binding to a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on a source.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "T">The Type of the property on the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on the source.</param>
        /// <returns><see cref = "IObserver{T}">IObserver</see> around the property on the source.</returns>
        public static IObserver<T> CreateOneWayToSource<TSource, T>(
            TSource source, ReactiveProperty<T> reactiveProperty) where TSource : ReactiveObject
        {
            return new OneWayToSourceReactivePropertyBinding<T, TSource>(source, reactiveProperty);
        }

        /// <summary>
        ///     Creates a One Way To Source Binding to a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on a source
        ///     where the Type of the source property is different from the Type of the target property and requires conversion.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "TSourceProp">The Type of the property on the source.</typeparam>
        /// <typeparam name = "TTargetProp">The Type of the property on the target.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on the source.</param>
        /// <returns><see cref = "IObserver{T}">IObserver</see> around the property on the source.</returns>
        public static IObserver<TTargetProp> CreateOneWayToSource<TSource, TSourceProp, TTargetProp>(
            TSource source, ReactiveProperty<TSourceProp> reactiveProperty) where TSource : ReactiveObject
        {
            return new OneWayToSourceReactivePropertyBinding<TTargetProp, TSourceProp, TSource>(
                source, reactiveProperty);
        }

        /// <summary>
        ///     Creates a Two Way Binding to a property on the element's Data Context.
        /// </summary>
        /// <remarks>
        ///     When binding to the Data Context, the binding returned is not resolved (ie. connected to the source) until the beginning of the Measure phase.
        ///     This allows for the Data Context to be set after the binding has been created and changed at any time.
        /// </remarks>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "T">The Type of the property on the Data Context.</typeparam>
        /// <param name = "propertySelector">Lambda expression which returns the property on the Data Context.</param>
        /// <returns><see cref = "IDualChannel{T}">IDualChannel</see> around the property on the Data Context.</returns>
        public static IDualChannel<T> CreateTwoWay<TSource, T>(Expression<Func<TSource, T>> propertySelector)
        {
            return new TwoWayBinding<T>(GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a Two Way Binding to a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on the element's Data Context.
        /// </summary>
        /// <remarks>
        ///     When binding to the Data Context, the binding returned is not resolved (ie. connected to the source) until the beginning of the Measure phase.
        ///     This allows for the Data Context to be set after the binding has been created and changed at any time.
        /// </remarks>
        /// <typeparam name = "TSource">The Type of the Data Context.</typeparam>
        /// <typeparam name = "T">The Type of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on the Data Context.</typeparam>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> you're binding to.</param>
        /// <returns><see cref = "IDualChannel{T}">IDualChannel</see> around the property on the source.</returns>
        public static IDualChannel<T> CreateTwoWay<TSource, T>(ReactiveProperty<T> reactiveProperty)
            where TSource : ReactiveObject
        {
            return new TwoWayReactivePropertyBinding<T, TSource>(reactiveProperty);
        }

        /// <summary>
        ///     Creates a Two Way Binding to a property on a source.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "T">The Type of the property on the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "propertySelector">Lambda expression which returns the property on the source.</param>
        /// <returns><see cref = "IDualChannel{T}">IDualChannel</see> around the property on the source.</returns>
        public static IDualChannel<T> CreateTwoWay<TSource, T>(
            TSource source, Expression<Func<TSource, T>> propertySelector)
        {
            return new TwoWayBinding<T>(source, GetPropertyInfo(propertySelector));
        }

        /// <summary>
        ///     Creates a Two Way Binding to a <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on a source.
        /// </summary>
        /// <typeparam name = "TSource">The Type of the source.</typeparam>
        /// <typeparam name = "T">The Type of the <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on the source.</typeparam>
        /// <param name = "source">The binding source.</param>
        /// <param name = "reactiveProperty">The <see cref = "ReactiveProperty{T}">ReactiveProperty</see> on the source.</param>
        /// <returns><see cref = "IDualChannel{T}">IDualChannel</see> around the property on the source.</returns>
        public static IDualChannel<T> CreateTwoWay<TSource, T>(TSource source, ReactiveProperty<T> reactiveProperty)
            where TSource : ReactiveObject
        {
            return new TwoWayReactivePropertyBinding<T, TSource>(source, reactiveProperty);
        }

        private static PropertyInfo GetPropertyInfo<T, TSource>(Expression<Func<TSource, T>> propertySelector)
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
