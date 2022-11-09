using ArsuLeo.CS.Utils.Model.Generic;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;

namespace ArsuLeo.CS.Utils.Model.Collections.Dictionary
{
    public class DuplicateKeyComparer<TKey> : IComparer<TKey>
        where TKey : IComparable
    {
        #region IComparer<TKey> Members

        public int Compare([AllowNull] TKey x, [AllowNull] TKey y)
        {
            int result = x is null ? y is null ? 0 : -1 : x.CompareTo(y);

            if (result == 0)
                return 1;   // Handle equality as beeing greater
            else
                return result;
        }

        #endregion
    }

    public class ReverseDuplicateKeyComparer<TKey> : DuplicateKeyComparer<TKey>
        where TKey : IComparable
    {
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public int Compare(TKey x, TKey y)
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        {
            return base.Compare(x, y) * (-1);
        }
    }

    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefaultNotNull<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue)
             where TKey : notnull
        {
            if (dic.TryGetValue(key, out TValue v))
            {
                return v;
            }
            return defaultValue;
        }

        //public static string Serialize(this IDictionary<string, string> dic)
        //{
        //    return JsonConvert.SerializeObject(dic, Formatting.Indented);
        //    //StringBuilder s = new StringBuilder();
        //    //foreach (KeyValuePair<string, string> kv in dic)
        //    //{
        //    //    if (kv.Key.Contains("="))
        //    //    {
        //    //        throw new ArgumentException($"Invalid key char \"=\" for key \"{kv.Key}\"");
        //    //    }
        //    //    if(StringUtil.NewLineRegex.IsMatch(kv.Value))
        //    //    {
        //    //        throw new ArgumentException($"Invalid key char \"=\" for key \"{kv.Key}\"");
        //    //    }
        //    //    s.Append(kv.Key + "=" + kv.Value + Environment.NewLine);
        //    //}
        //    //return s.ToString();
        //}

        //public static string Serialize(this IDictionary<string, object> dic)
        //{
        //    return JsonConvert.SerializeObject(dic, Formatting.Indented);
        //}

        //public static string Serialize<T>(this IDictionary<string, T> dic)
        //{
        //    return JsonConvert.SerializeObject(dic, Formatting.Indented);
        //}

        public static Dictionary<string, T> WithoutNull<T>(this IDictionary<string, T?> dic)
            where T : class
        {
            Dictionary<string, T> result = new Dictionary<string, T>();
            foreach (KeyValuePair<string, T?> pair in dic)
            {
                if (pair.Value != null)
                {
                    result.Add(pair.Key, pair.Value);
                }
            }
            return result;
        }

        public static Dictionary<string, string> RemoveNullOrEmpty(this IDictionary<string, string?> dic)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string?> pair in dic)
            {
                if (!string.IsNullOrEmpty(pair.Value))
                {
                    result.Add(pair.Key, pair.Value);
                }
            }
            return result;
        }

        public static void Each<TKey, TValue>(this IDictionary<TKey, TValue> dic, Action<TKey, TValue> eachFn)
            where TKey : notnull
        {
            foreach (KeyValuePair<TKey, TValue> pair in dic)
            {
                eachFn(pair.Key, pair.Value);
            }
        }
        public static Dictionary<TResultKey, TResultValue> Map<TResultKey, TResultValue, TInputKey, TInputValue>
            (this IDictionary<TInputKey, TInputValue> dic, Action<TInputKey, TInputValue, Dictionary<TResultKey, TResultValue>> mapFn)
            where TInputKey : notnull
            where TResultKey : notnull
        {
            Dictionary<TResultKey, TResultValue> result = new Dictionary<TResultKey, TResultValue>();
            dic.Each((TInputKey key, TInputValue val) => mapFn(key, val, result));

            return result;
        }

        public static ImmutableDictionary<TKey, TValue> EmptyInmutableDictionary<TKey, TValue>()
            where TKey : notnull
        {
            return new Dictionary<TKey, TValue>().ToImmutableDictionary();
        }

        //public static Dictionary<string, string> Unserialize(string s)
        //{
        //    return JsonConvert.DeserializeObject<Dictionary<string, string>>(s) ?? new Dictionary<string, string>();
        //    //Dictionary<string, string> result = new Dictionary<string, string>();
        //    //foreach (string line in StringUtil.SplitStringByAnyNewLines(s, StringSplitOptions.RemoveEmptyEntries))
        //    //{
        //    //    string[] parts = line.Split(new[] { '=' });
        //    //    string key = parts[0];
        //    //    string value = parts.Length > 1 ? string.Join("=", parts.Skip(1)) : "";
        //    //    result.Add(key, value);
        //    //}
        //    //return result;
        //}

        /// <summary>
        /// The first dicionary keys have preference
        /// </summary>
        /// <param name="main"></param>
        /// <param name="dics"></param>
        public static void MergeIntoMain<TKey, TValue>(Dictionary<TKey, TValue> main, IEnumerable<IDictionary<TKey, TValue>> dics)
            where TKey : notnull
        {
            using IEnumerator<IDictionary<TKey, TValue>> enu = dics.GetEnumerator();
            while (enu.MoveNext())
            {
                MergeIntoMain(main, enu.Current);
            }
        }

        /// <summary>
        /// The first dicionary keys have preference
        /// </summary>
        /// <param name="main"></param>
        /// <param name="dics"></param>
        public static void MergeIntoMain<TKey, TValue>(Dictionary<TKey, TValue> main, IDictionary<TKey, TValue> dic)
            where TKey : notnull
        {
            foreach (KeyValuePair<TKey, TValue> p in dic)
            {
                if (!main.ContainsKey(p.Key))
                {
                    main.Add(p.Key, p.Value);
                }
            }
        }

        public static Dictionary<TKey, TValue> MergeIntoNewPrams<TKey, TValue>(params IDictionary<TKey, TValue>[] dics)
            where TKey : notnull
        {
            return MergeIntoNewEnum(dics);
        }
        /// <summary>
        /// The first dictionary keys have preference
        /// </summary>
        /// <param name="dics"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> MergeIntoNewEnum<TKey, TValue>(IEnumerable<IDictionary<TKey, TValue>> dics)
        where TKey : notnull
        {
            using IEnumerator<IDictionary<TKey, TValue>> enu = dics.GetEnumerator();
            if (!enu.MoveNext())
            {
                return new Dictionary<TKey, TValue>();
            }
            Dictionary<TKey, TValue> result = DictionaryCloneExtensions.CloneDictionary(enu.Current);
            while (enu.MoveNext())
            {
                MergeIntoMain(result, enu.Current);
            }
            return result;
        }



        public static bool Equals<TKey, TValue>(IDictionary<TKey, TValue> a, IDictionary<TKey, TValue> b)
            where TKey : notnull
        {
            if (a.Count != b.Count)
            {
                return false;
            }
            foreach (KeyValuePair<TKey, TValue> kv in a)
            {
                if (!b.ContainsKey(kv.Key))
                {
                    return false;
                }
                if (Equals(kv.Value, b[kv.Key]))
                {
                    return false;
                }
            }
            return true;
        }

        public static List<Pair<T1, T2>> ToPairList<T1, T2>(this IDictionary<T1, T2> dic)
            where T1 : notnull
        {
            List<Pair<T1, T2>> result = new List<Pair<T1, T2>>();
            foreach (KeyValuePair<T1, T2> pair in dic)
            {
                result.Add(new Pair<T1, T2>(pair.Key, pair.Value));
            }
            return result;
        }

        //public static List<StrPair> DicToPairList(Dictionary<string, string> dic)
        //{
        //    List<StrPair> result = new List<StrPair>();
        //    foreach (KeyValuePair<string, string> pair in dic)
        //    {
        //        result.Add(new StrPair(pair.Key, pair.Value));
        //    }
        //    return result;
        //}

        public static bool TryGetValue(this ExpandoObject? obj, string propertyName, out object? value)
        {
            if (obj is object && ((IDictionary<string, object>)obj).TryGetValue(propertyName, out value) && value is object)
            {
                return true;
            }
            value = null;
            return false;
        }
    }
}
