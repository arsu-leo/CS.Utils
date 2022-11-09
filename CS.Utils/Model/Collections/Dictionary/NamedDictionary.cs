using ArsuLeo.CS.Utils.Model.Generic;
using System;
using System.Collections.Generic;

namespace ArsuLeo.CS.Utils.Model.Collections.Dictionary
{
    public class NamingDictionary<TKey, TValue> : Dictionary<TKey, TValue>
        where TKey : notnull
    {
        public readonly Func<TValue, TKey> KFunc;

        public NamingDictionary(Func<TValue, TKey> kFunc)
        {
            KFunc = kFunc;
        }

        public TValue this[TValue kValue]
        {
            get
            {
                return this[KFunc(kValue)];
            }
        }

        public void Set(TValue element)
        {
            this[KFunc(element)] = element;
        }

        public void Add(TValue element)
        {
            TKey value = KFunc(element);
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Add(value, element);
        }

        public TKey GetKey(TValue v)
        {
            return KFunc(v);
        }

        public NamingDictionary<TKey, TValue> CloneNaming()
        {
            return FromEnum(Values, KFunc);
        }

        #region Builders
        public static NamingDictionary<IKey, IValue> FromDictionary<IKey, IValue>(IDictionary<IKey, IValue> d, Func<IValue, IKey> kFunc)
            where IKey : notnull
        {
            return FromEnum(d.Values, kFunc);
        }

        public static NamingDictionary<IKey, IValue> FromEnum<IKey, IValue>(IEnumerable<IValue> src, Func<IValue, IKey> kFunc)
            where IKey : notnull
        {
            NamingDictionary<IKey, IValue> d = new NamingDictionary<IKey, IValue>(kFunc);
            using IEnumerator<IValue> enu = src.GetEnumerator();
            while (enu.MoveNext())
            {
                IValue value = enu.Current;
                IKey key = kFunc(value);
                if (key is null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                d.Add(key, value);
            }
            return d;
        }
        #endregion Builders
    }

    public class NamedDictionary<TValue> : NamingDictionary<string, TValue>
        where TValue : INamedObject
    {
        private static readonly Func<TValue, string> DFunc = (n) => n.Name;
        public NamedDictionary() : base(DFunc)
        {

        }

        public NamedDictionary<TValue> CloneNamed()
        {
            return FromEnum(Values);
        }

        public static NamedDictionary<IValue> FromEnum<IValue>(IEnumerable<IValue> src)
            where IValue : INamedObject
        {
            NamedDictionary<IValue> d = new NamedDictionary<IValue>();
            using IEnumerator<IValue> enu = src.GetEnumerator();
            while (enu.MoveNext())
            {
                IValue value = enu.Current;
                d.Add(value);
            }
            return d;
        }

    }
}
