using System;
using System.Collections.Generic;

namespace ArsuLeo.CS.Utils.Model.Collections.Dictionary
{
    public class DictionaryBasedCache<T1, T2>
        where T1 : notnull
    {

        private readonly Dictionary<T1, T2> Cache;

        private readonly Func<T1, T2> ElementFactory;
        public DictionaryBasedCache(Func<T1, T2> elementFactory)
        {
            Cache = new Dictionary<T1, T2>();
            ElementFactory = elementFactory;
        }

        public DictionaryBasedCache(Dictionary<T1, T2> initialData, Func<T1, T2> elementFactory)
        {
            Cache = new Dictionary<T1, T2>(initialData);
            ElementFactory = elementFactory;
        }


        public T2 GetValue(T1 t1, bool forceRefreshCache = false)
        {
            if (!forceRefreshCache && Cache.TryGetValue(t1, out T2 v))
            {
                return v;
            }
            v = ElementFactory(t1);
            Cache[t1] = v;
            return v;
        }

    }
}
