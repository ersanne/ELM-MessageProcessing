using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ELMPrototype.bdo
{
    /// <summary>
    ///     ObservableCollection sort implementation
    /// </summary>
    public static class ObservableCollection
    {
        public static void Sort<TSource, TKey>(this ObservableCollection<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var sortedList = source.OrderByDescending(keySelector).ToList();
            source.Clear();
            foreach (var sortedItem in sortedList) source.Add(sortedItem);
        }
    }
}