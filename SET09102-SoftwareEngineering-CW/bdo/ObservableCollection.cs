using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SET09102_SoftwareEngineering_CW.bdo
{
    public static class ObservableCollection
    {
        public static void Sort<TSource, TKey>(this ObservableCollection<TSource> source, Func<TSource, TKey> keySelector)
        {
            List<TSource> sortedList = source.OrderByDescending(keySelector).ToList();
            source.Clear();
            foreach (var sortedItem in sortedList)
            {
                source.Add(sortedItem);
            }
        }
    }
}