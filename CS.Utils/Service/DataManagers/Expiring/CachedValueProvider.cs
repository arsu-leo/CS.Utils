using System.Diagnostics.CodeAnalysis;

namespace ArsuLeo.CS.Utils.Service.DataManagers.Expiring
{
    public class CachedValueProvider<T>
        where T : class
    {
        private string LastCacheKey = string.Empty;
        private T? CachedValue;
        private readonly object LockObj = new object();

        public CachedValueProvider()
        {
        }

        public bool TryGetValue(string cachekey, [NotNullWhen(true)] out T? cachedVal)
        {
            lock (LockObj)
            {
                T? oldV = CachedValue;
                if (LastCacheKey.Equals(cachekey) && oldV != null)
                {
                    cachedVal = oldV;
                    return true;
                }
                cachedVal = null;
                return false;
            }
        }

        public T SetValue(string cacheKey, T cachValue)
        {
            lock (LockObj)
            {
                LastCacheKey = cacheKey;
                CachedValue = cachValue;
                return CachedValue;
            }
        }
    }
}
