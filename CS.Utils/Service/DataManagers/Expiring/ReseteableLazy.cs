using System;

namespace ArsuLeo.CS.Utils.Service.DataManagers.Expiring
{
    /// <summary>
    /// Provides a Lazy<T> but can be manually reset
    /// </summary>
    public class ReseteableLazy<T>
    {
        public bool IsValueCreated
        {
            get { return LastValue.IsValueCreated; }
        }
        
        private Lazy<T> LastValue;
        private readonly Func<T> Factory;
        public ReseteableLazy(Func<T> factory)
        {
            Factory = factory;
            LastValue = new Lazy<T>(factory);
        }

        public T Value
        {
            get
            {
                    return LastValue.Value;
            }
        }

        public void Reset()
        {
            LastValue = new Lazy<T>(Factory);
        }
    }
}
