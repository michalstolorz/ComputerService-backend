using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Extensions
{
    public static class IEnumerableExtenions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
               this IEnumerable<TSource> source,
               Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> keys = new HashSet<TKey>();

            foreach (TSource item in source)
            {
                if (keys.Add(keySelector(item)))
                {
                    yield return item;
                }
            }
        }
    }
}
