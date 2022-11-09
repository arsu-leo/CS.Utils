using System;

namespace ArsuLeo.CS.Utils.Service.DataManagers
{
    public class ValueManager<T>
    {
        private readonly Action<T> ValueSetter;

        private readonly object LockObj;

        public ValueProvider<T> Provider { get; }
        public ValueManager(Func<T> valueGetter, Action<T> valueSetter, object lockObj)
        {
            ValueSetter = valueSetter;
            Provider = new ValueProvider<T>(valueGetter);
            LockObj = lockObj;
        }

        public T GetValue()
        {
            return Provider.GetValue();
        }

        public void SetValue(T v)
        {
            lock (LockObj)
            {
                ValueSetter(v);
            }
        }

        //Provides a method to get a value and optionaly change it while locking to avoid other changes, also returns the same as the provided func returns
        public T LockForSaveReturnsValue(Func<T, Action<T>, T> valueProviderWithSaveCallbackReturnsFnVal)
        {
            lock (LockObj)
            {
                T currentValue = GetValue();
                return valueProviderWithSaveCallbackReturnsFnVal(currentValue, ValueSetter);
            }
        }
        //Provides a method to get a value and optionaly change it while locking to avoid other changes
        public void LockForSaveCallback(Action<T, Action<T>> runFn)
        {
            lock (LockObj)
            {
                T currentValue = GetValue();
                runFn(currentValue, ValueSetter);
            }
        }
    }
}
