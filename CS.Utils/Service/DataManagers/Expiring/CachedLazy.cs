using System;

namespace ArsuLeo.CS.Utils.Service.DataManagers.Expiring
{
    /// <summary>
    /// Provides a Lazy<T> but can be manually reset and has a specific live time
    /// </summary>
    public class CachedLazy<T>
    {
        public bool IsValueCreated => LastValue.IsValueCreated;
        public bool IsValueAlive
        {
            get
            {
                return IsValueCreated && ExpireDate >= DateTime.Now;
            }
        }

        public DateTime ExpireDate { get; private set; } = DateTime.MinValue;

        public TimeSpan LiveTime { get; }

        private readonly ReseteableLazy<T> LastValue;

        public CachedLazy(Func<T> factory, TimeSpan liveTime)
        {
            LiveTime = liveTime;
            LastValue = new ReseteableLazy<T>(factory);
        }

        public T Value
        {
            get
            {
                if (!IsValueAlive && LastValue.IsValueCreated)
                {
                    ResetValue();
                }
                if (!LastValue.IsValueCreated)
                {
                    ExpireDate = DateTime.Now.Add(LiveTime);
                }
                return LastValue.Value;
            }
        }

        public void ResetValue()
        {
            LastValue.Reset();
        }
    }
}
