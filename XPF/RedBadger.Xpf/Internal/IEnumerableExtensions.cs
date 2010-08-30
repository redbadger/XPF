namespace RedBadger.Xpf.Internal
{
    using System;
    using System.Collections.Generic;

    internal static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }
    }
}