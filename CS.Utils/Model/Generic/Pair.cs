using System;

namespace ArsuLeo.CS.Utils.Model.Generic
{
    public class Pair<T1, T2>
    {
        private readonly (T1, T2) _tuple;
        public T1 Key => _tuple.Item1;
        public T2 Value => _tuple.Item2;

        public Pair(T1 key, T2 value)
        {
            _tuple = (key, value);
        }

        public (T1, T2) Deconstruct()
        {
            return (Key, Value);
        }
        public void Deconstruct(out T1 key, out T2 value)
        {
            key = Key;
            value = Value;
        }

        //public object?[] ToArray()
        //{
        //    return new object?[] { Key, Value };
        //}
    }
    public class StrPair : Pair<string, string>
    {
        public StrPair(string key, string value) : base(key, value)
        {
        }

        public string[] ToStrArray()
        {
            return new string[] { Key, Value };
        }
    }
}
