using System;

namespace ArsuLeo.CS.Utils.Service.DataManagers
{
    public class ValueProvider<T>
    {
        private readonly Func<T> ValueGetter;
        public ValueProvider(Func<T> valueGetter)
        {
            ValueGetter = valueGetter;
        }
        public virtual T GetValue() => ValueGetter();
    }
}
