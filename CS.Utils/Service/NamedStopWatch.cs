using System;
using System.Diagnostics;

namespace ArsuLeo.CS.Utils.Service
{
    public class NamedStopWatch : Stopwatch
    {
        public string Name { get; private set; }
        public NamedStopWatch(string name)
        {
            Name = name;
        }

        public static NamedStopWatch StartNew(string name)
        {
            NamedStopWatch st = new NamedStopWatch(name);
            st.Restart();

            return st;
        }

        public string StopAndResult()
        {
            Stop();
            return $"{Name}: {ElapsedMilliseconds}ms";
        }

        public static string Run(string name, Action fn)
        {
            NamedStopWatch s = StartNew(name);
            fn();
            return s.StopAndResult();
        }

        public static (T value, string timming) RunFunc<T>(string name, Func<T> valueFactory)
        {
            NamedStopWatch s = NamedStopWatch.StartNew(name);
            T r = valueFactory();
            return (r, s.StopAndResult());
        }

        public static (T value, string timming) RunFunc<T>(Func<T> valueFactory)
        {
            Type? t = valueFactory.Method.ReflectedType;
            string fullName = t == null ? "NullTypeFullName" : t.FullName ?? "NullFullNameType";
            return RunFunc(fullName + ":" + valueFactory.Method.Name, valueFactory);
        }
    }
}
