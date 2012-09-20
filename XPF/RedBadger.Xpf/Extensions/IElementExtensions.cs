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

        /// <summary>
        ///     Assert whether this element is a descendant of the specified ancestor.
        /// </summary>
        /// <param name = "element">This instance of <see cref = "IElement">IElement</see>.</param>
        /// <param name = "ancestor">The ancestor to query against.</param>
        /// <returns>True if this element is a descendant of the ancestor.</returns>
        public static bool IsDescendantOf(this IElement element, IElement ancestor)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (ancestor == null)
            {
                throw new ArgumentNullException("ancestor");
            }

            IElement parent = element.VisualParent;
            while (parent != null)
            {
                if (parent == ancestor)
                {
                    return true;
                }

                parent = parent.VisualParent;
            }

            return false;
        }
    }
}
