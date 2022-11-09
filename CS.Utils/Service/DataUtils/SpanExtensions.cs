using System;
using System.Collections.Generic;

namespace ArsuLeo.CS.Utils.Service.DataUtils
{
    public static class SpanExtensions
    {
        //public static T[] ToArray<T>(this ReadOnlySpan<T> data)
        //{
        //    T[] arr =  new T[data.Length];
        //    data.CopyTo(arr);
        //    return arr;
        //}
        public static Span<T> GetReverse<T>(this ReadOnlySpan<T> data)
        {
            Span<T> sp = data.ToArray(); //I would need an array anyways
            sp.Reverse();
            return sp;
        }
        
        /// <summary>
        /// Returns number of elements written
        /// </summary>
        //public static int Write<T>(this Span<T> sp, int pos, ReadOnlySpan<T> data) => sp.Write(data, pos);

        public static int Write<T>(this Span<T> sp, int startIndex, ReadOnlySpan<T> data)
        {
            Span<T> slice = startIndex == 0 ? sp : sp.Slice(startIndex);
            data.CopyTo(slice);
            return Math.Min(slice.Length, data.Length);
        }

        /// <summary>
        /// Returns number of elements written
        /// </summary>
        public static int Write<T>(this Span<T> sp, ReadOnlySpan<T> data) => Write(sp, 0, data);
        

        public static int Write<T>(this Span<T> sp, IEnumerable<T> data, in int startIndex = 0)
        {
            int n = 0;
            IEnumerator<T> en = data.GetEnumerator();
            int index = startIndex;
            while(en.MoveNext() && index < sp.Length)
            {
                sp[index] = en.Current;
                n++;
                index++;
            }
            return n;
        }

        public static int WriteTo<T>(this ReadOnlySpan<T> sp, Span<T> destinator, int startIndex)
        {
            sp.CopyTo(startIndex == 0 ?  destinator : destinator.Slice(startIndex));
            return sp.Length;
        }
    }
}
