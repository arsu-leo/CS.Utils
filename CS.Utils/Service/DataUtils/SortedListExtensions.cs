using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ArsuLeo.CS.Utils.Service.DataUtils
{
    public static class SortedListExtensions
    {
        public static bool TryFind<TKey, TValue>(this SortedList<TKey, TValue> list, Func<TValue, bool> fn, out int index, [NotNullWhen(true)] out TKey key, out TValue value)
            where TKey : notnull
        {
            IEnumerator<KeyValuePair<TKey, TValue>> enumerator = list.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
                if (fn(enumerator.Current.Value))
                {
                    index = i;
                    key = enumerator.Current.Key;
                    value = enumerator.Current.Value;
                    return true;
                }
                i++;
            }
            index = -1;
#pragma warning disable CS8601 // Possible null reference assignment.
            key = default;
            value = default;
#pragma warning restore CS8601 // Possible null reference assignment.
            return false;
        }
    }
}
