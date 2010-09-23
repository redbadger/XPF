namespace RedBadger.Xpf.Extensions
{
    using System;

    /// <summary>
    ///     IElement Extensions for XPF.
    /// </summary>
    public static class IElementExtensions
    {
        /// <summary>
        ///     Returns the nearest ancestor of the specified type, which maybe itself or null if there is no ancestor of the requested type.
        /// </summary>
        /// <typeparam name = "T">The Type of the ancestor to find.</typeparam>
        /// <param name = "element">The element from which to start the search.</param>
        /// <returns>The nearest ancestor of Type T</returns>
        public static IElement FindNearestAncestorOfType<T>(this IElement element)
        {
            return element.FindNearestAncestorOfType(typeof(T));
        }

        /// <summary>
        ///     Returns the nearest ancestor of the specified type, which maybe itself or null if there is no ancestor of the requested type.
        /// </summary>
        /// <param name = "element">The element from which to start the search.</param>
        /// <param name = "ancestorType">The Type of the ancestor to find.</param>
        /// <returns>The nearest ancestor of Type specified</returns>
        public static IElement FindNearestAncestorOfType(this IElement element, Type ancestorType)
        {
            if (ancestorType == null)
            {
                throw new ArgumentNullException("ancestorType");
            }

            IElement ancestor = element;
            while (ancestor != null && !ancestorType.IsAssignableFrom(ancestor.GetType()))
            {
                ancestor = ancestor.VisualParent;
            }

            return ancestor;
        }
    }
}