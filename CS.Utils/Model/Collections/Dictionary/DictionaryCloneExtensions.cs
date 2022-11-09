using System;
using System.Collections.Generic;
using System.Linq;

namespace ArsuLeo.CS.Utils.Model.Collections.Dictionary
{
    public static class DictionaryCloneExtensions
    {


        /// <summary>
        /// Inner references are not cloned
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>(this IDictionary<TKey, TValue> src)
            where TKey : notnull
        {
            Dictionary<TKey, TValue> dst = new Dictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> kv in src)
            {
                dst.Add(kv.Key, kv.Value);
            }
            return dst;
        }

        public static Dictionary<TKey, TValue> CloneDictionarySkipKeys<TKey, TValue>(this IDictionary<TKey, TValue> src, params TKey[] removeKeys)
       where TKey : notnull
        {
            Dictionary<TKey, TValue> dst = new Dictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> kv in src)
            {
                if (!removeKeys.Contains(kv.Key))
                {
                    dst.Add(kv.Key, kv.Value);
                }
            }
            return dst;

        }

        public static Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>(this IDictionary<TKey, TValue> src, Func<TValue, TValue> valueClone)
            where TKey : notnull
        {
            Dictionary<TKey, TValue> dst = new Dictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> kv in src)
            {
                dst.Add(kv.Key, valueClone(kv.Value));
            }
            return dst;
        }

        public static Dictionary<TKey, TValue> CloneDictionarySkipKeys<TKey, TValue>(this IDictionary<TKey, TValue> src, Func<TValue, TValue> valueClone, params TKey[] removeKeys)
            where TKey : notnull
        {
            Dictionary<TKey, TValue> dst = new Dictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> kv in src)
            {
                if (!removeKeys.Contains(kv.Key))
                {
                    dst.Add(kv.Key, valueClone(kv.Value));
                }
            }
            return dst;
        }






    }
}
