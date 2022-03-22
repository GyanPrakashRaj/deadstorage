using System;
using System.Collections.Generic;

namespace WowDash
{
    public static class Utilities
    {
        /// <summary>
        /// Selects distinct elements from a list by one of their properties.
        /// Bypasses the need to implement .Equals() and .HashCode() unnecessarily.
        /// Implemented for cleaner list filtering from:
        /// https://stackoverflow.com/questions/489258/linqs-distinct-on-a-particular-property
        /// </summary>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}